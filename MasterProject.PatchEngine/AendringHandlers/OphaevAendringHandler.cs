using System;
using System.Linq;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace MasterProject.PatchEngine.AendringHandlers
{
    public class OphaevAendringHandler
    {
        private readonly AendringDefinition _aendringDefinition;
        private readonly XElement _changeItem1;

        public OphaevAendringHandler(AendringDefinition aendringDefinition, XElement changeItem1)
        {
            if(aendringDefinition.AktionType!=AktionType.Ophaev)
                throw new InvalidOperationException($"Aendring aktion for this handler should be {AktionType.Ophaev}, but is {aendringDefinition.AktionType}.");
            _aendringDefinition = aendringDefinition;
            _changeItem1 = changeItem1;
        }
        public void Apply(TargetDocument targetDocument)
        {
            foreach (var aendringDefinitionTarget in _aendringDefinition.Targets)
            {
                var targetChain = aendringDefinitionTarget.GetAncestorsAndSelf.Reverse().ToArray();
                Element next = targetChain.First();
                XElement searchScope = targetDocument.Source.Root;
                while (next!=null)
                {
                    var elementsFound= searchScope
                        .Descendants(next.LexdaniaName)
                        .Where(element => next.NummerMatch.IsMatch(element.Element("Explicatus")?.Value??string.Empty))
                        .ToArray();
                    ValidateFoundElements(elementsFound, next);
                    searchScope = elementsFound.First().AncestorsAndSelf(next.LexdaniaName).First();
                    var nextIndex = Array.IndexOf(targetChain, next)+1;
                    next = nextIndex>=targetChain.Length?null: targetChain[nextIndex];
                }
                //act
                searchScope?.Remove();
            }
        }

        private static void ValidateFoundElements(XElement[] elementsFound, Element next)
        {
            if (!elementsFound.Any())
                throw new ApplyAendringerException($"No elements found for change {next}");
            if (elementsFound.Length > 1)
                throw new ApplyAendringerException($"More than element found for change {next}");
        }
    }
}