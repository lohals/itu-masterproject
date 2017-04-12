using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;
using Dk.Itu.Rlh.MasterProject.Parser;
using Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition;
using Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning;
using MasterProject.PatchEngine.AendringHandlers;

namespace MasterProject.PatchEngine.PatchTasks
{
    public class ApplyAendringAktioner : IPatchTask
    {
        private ParagrafIndledningParser _paragrafIndledningParser;
        private AendringDefintionParser _aendringDefinitiontParser;
        private AendringAktionHandlerFactory _aendringAendringAktionHandlerFactory;

        public ApplyAendringAktioner()
        {
            _paragrafIndledningParser = new ParagrafIndledningParser();
            _aendringDefinitiontParser = new AendringDefintionParser();
            _aendringAendringAktionHandlerFactory = new AendringAktionHandlerFactory();
        }
        public void Patch(TargetDocument targetDocument, ChangeDocument[] changes)
        {
            var chronologicalOrder = changes
                .OrderBy(change => change.Year)
                .ThenBy(change => change.Number)
                .Select(change => new {change, RelevantChanges = FindRelevantChanges(targetDocument, change)})
                .ToArray();

            foreach (var change in chronologicalOrder)
            {
                ApplyChange(change.RelevantChanges, targetDocument);
            }
        }

        private void ApplyChange(Tuple<XElement, AendringDefinition>[] relevantChanges, TargetDocument targetDocument)
        {
            foreach (var change in relevantChanges)
            {
                _aendringAendringAktionHandlerFactory
                    .Create(change.Item2, change.Item1)
                    .Apply(targetDocument);
            }
        }

        private Tuple<XElement,AendringDefinition>[] FindRelevantChanges(TargetDocument targetDocument, ChangeDocument change)
        {
            var aendringer = "//AendringCentreretParagraf/Exitus[1]";
            
            //Parse all paragrafindlendinger asynchronously
            var allParagrafIndledninger = Task.WhenAll(change.XDoc.XPathSelectElements(aendringer)
                .Select(element => new { AendringCentreretParagrafNode=element.Parent, ParagrafText = element.Value })//Map to Paragraf Node and paragrafindledning text
                .Select(s => Task.Run(()=>new {s.AendringCentreretParagrafNode, ParseResult=_paragrafIndledningParser.Parse(s.ParagrafText)})) //parse paragrafindledning
                .ToArray());

            allParagrafIndledninger.Wait();

            //find aendrings paragraffer that points to the target document
            var allParagrafNodesThatPointsToTargetDocument = allParagrafIndledninger.Result
                .Where(map => !map.ParseResult.ErrorResult.HasErrors)
                .Where(map =>ParagrafIndledningerThatPointsToTargetDocument(targetDocument,map.ParseResult))
                .Select(arg => arg.AendringCentreretParagrafNode)
                .ToArray();

            //parse all aendringer in each paragrafindledninger
            return allParagrafNodesThatPointsToTargetDocument.Descendants("Aendring")
                .Where(element => element.Elements("AendringDefinition").Any())
                .Select(element => new { Aendring=element, ParseResult=_aendringDefinitiontParser.Parse(element.Value.Trim())})
                .Where(map => !map.ParseResult.ErrorResult.HasErrors)
                .Select(map => new Tuple<XElement,AendringDefinition>(map.Aendring,map.ParseResult.Result))
                .ToArray();
        }


        public bool ParagrafIndledningerThatPointsToTargetDocument(TargetDocument targetDocument,ParseResult<ParagrafIndledningModel> parseResult)
        {
            var resultReference = parseResult.Result.Reference;
            return
                resultReference.DokumentReference.DokumentType == targetDocument.DokumentType
                && resultReference.DokumentReference.Number == targetDocument.Number
                && resultReference.DokumentReference.Year == targetDocument.Year;
        }
    }
}