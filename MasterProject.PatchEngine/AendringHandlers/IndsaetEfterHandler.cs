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
        private readonly AendringDefinition _aendringDefinition;
        private readonly XElement _changeDefinition;

        public IndsaetEfterHandler(AendringDefinition aendringDefinition, XElement changeDefinition) : base(aendringDefinition,changeDefinition)
        {
            if(aendringDefinition.AktionType!=AktionType.IndsaetEfter)
                throw new ApplyAendringerException($"AktionType for this handler should be {aendringDefinition.AktionType}.");
            _aendringDefinition = aendringDefinition;
            _changeDefinition = changeDefinition;
        }

        public void Apply(TargetDocument targetDocument)
        {
            var elementToInsert = AendringElement.Element("AendringAktion").Element("AendringNyTekst").Elements();

            foreach (var xElement in FindElementInTarget(targetDocument))
            {
                var xName = xElement.Name;
                XElement siblingToInsertAfter = xElement;
                var elementsCommonElementName = elementToInsert.First().Name;
                if (xName != elementsCommonElementName)
                    siblingToInsertAfter = xElement.Ancestors(elementsCommonElementName).First();

                siblingToInsertAfter.AddAfterSelf(elementToInsert.NormalizeWhiteSpace());
            }
        }
    }
}