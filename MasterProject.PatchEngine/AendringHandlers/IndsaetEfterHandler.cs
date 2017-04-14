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
            var saetninger = GetSupportedSubStructureTargets();
            var aendringAktionChange = GetAendringAktionStructures().ToArray();
            foreach (var saetning in saetninger)
            {
               
                var structureElementsFound = GetTargetStructureForSubstructureChange(targetDocument, saetning);
                if(structureElementsFound.Length==0)
                    throw new ApplyAendringerException($"No target structure found for {saetning}, can't apply subelement changes.");

                DoSubStructureInsertAfter(structureElementsFound, saetning, aendringAktionChange);
            }
        }

        private static void DoSubStructureInsertAfter(XElement[] structureElementsFound, Saetning saetning,
            IEnumerable<XElement> aendringAktionChange)
        {
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
                if (targetChar == null)
                    throw new ApplyAendringerException("No char elemenet is found. Something is wrong.");

                var targetLinea = targetChar.Parent;
                targetLinea.AddAfterSelf(aendringAktionChange.Descendants("Linea"));
            }
        }


        private void HandleStructureElementInserts(TargetDocument targetDocument)
        {
            var elementToInsert = GetAendringAktionStructures();

            var structureTargets = AendringDefinition.StructureTargets.Where(element => element.SubElementTarget==null);
            foreach (var xElement in FindStructureElementsInTargetDocument(targetDocument, structureTargets))
            {
                //We only insert after same type... sometimes, forexample for Paragrafgrupper the location is a paragraf, but the change is a paragrafgruppe
                //it is not allowed to insert paragrafgrupper inside paragrafgrupper, so that is why we select first ancestor with same name as change being inserted
                var siblingToInsertAfter = SelectSameTypeSelfOrImmediateAncestor(xElement, elementToInsert);

                siblingToInsertAfter.AddAfterSelf(elementToInsert.NormalizeWhiteSpace());
            }
        }
    }
}