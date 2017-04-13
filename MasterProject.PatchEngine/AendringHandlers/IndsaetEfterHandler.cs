using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;
using MasterProject.Utilities;

namespace MasterProject.PatchEngine.AendringHandlers
{
    public class IndsaetEfterHandler : BaseAendringAktionHandler,IAendringAktionHandler
    {
        public IndsaetEfterHandler(AendringDefinition aendringDefinition, XElement changeDefinition) : base(aendringDefinition,changeDefinition)
        {
            if(aendringDefinition.AktionType!=AktionType.IndsaetEfter)
                throw new ApplyAendringerException($"AktionType for this handler should be {aendringDefinition.AktionType}.");
        }

        public void Apply(TargetDocument targetDocument)
        {
         
            HandleStructureElementInserts(targetDocument);

           HandleSubElementInserts(targetDocument);

        }
        private void HandleSubElementInserts(TargetDocument targetDocument)
        {
            var targets = AendringDefinition.Targets.Where(element => !element.IsStructureElement);
            var aendringAktionChange = GetAendringAktionStructures();
            foreach (var element in targets)
            {
                var saetning = element as Saetning;
                if(saetning==null)//we only support saetninger for now.
                    continue;
                
                var targetStructure = element.GetAncestorsAndSelf.FirstOrDefault(e => e.IsStructureElement);

                var structureElementsFound = FindStructureElementsInTargetDocument(targetDocument, targetStructure).ToArray();
                if(structureElementsFound?.Length==0)
                    throw new ApplyAendringerException($"No target structure found for {element}, can't apply subelement changes.");

                foreach (var xElement in structureElementsFound)
                {
                    var punktumPerCharMap = xElement.Descendants("Char")
                        .Select(charElement => new
                        {
                            Char = charElement,
                            NumberOfPunktum = charElement.Value.Count(c => c == '.')
                        });
                    int acc = 0;
                    XElement targetChar = null;
                    using (var enumerator = punktumPerCharMap.GetEnumerator())
                    {
                        while (acc <= saetning.NummerStrong && enumerator.MoveNext())
                        {
                            acc = acc + enumerator.Current.NumberOfPunktum;
                            targetChar = enumerator.Current.Char;
                        }
                    }
                    if(targetChar==null)
                        throw new ApplyAendringerException("No char elemenet is found. Something is wrong.");

                    var targetLinea = targetChar.Parent;
                    targetLinea.AddAfterSelf(aendringAktionChange.Descendants("Linea"));



                    var existingText = xElement.Value;
                    int indexOfPunktumToInsertAfter = 0;
                    for (int i = 0; i < saetning.NummerStrong; i++)
                    {
                        indexOfPunktumToInsertAfter = existingText.IndexOf('.', indexOfPunktumToInsertAfter);
                    }
                    //foreach (var elementSubElementTarget in element.SubElementTargets?? aendringAktionChange)
                    //{
                    //    var chars = xElement.Descendants("Char");
                    //    foreach (var c in chars)
                    //    {
                    //        c.Value = c.Value.Replace(elementSubElementTarget.Target, elementSubElementTarget.Replacement);
                    //    }
                    //}
                }
            }


        }

        private string GetInsertion()
        {
            throw new NotImplementedException();
        }


        private void HandleStructureElementInserts(TargetDocument targetDocument)
        {
            var elementToInsert = GetAendringAktionStructures();

            foreach (var xElement in FindStructureElementsInTargetDocument(targetDocument, AendringDefinition.StructureTargets))
            {
                var siblingToInsertAfter = SelectSameTypeSelfOrImmediateAncestor(xElement, elementToInsert);

                siblingToInsertAfter.AddAfterSelf(elementToInsert.NormalizeWhiteSpace());
            }
        }
    }
}