using System;
using System.Linq;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace MasterProject.PatchEngine.AendringHandlers
{
    public class OphaevAendringHandler : BaseAendringAktionHandler, IAendringAktionHandler
    {

        public OphaevAendringHandler(AendringDefinition aendringDefinition, XElement aendringElement) :base(aendringDefinition,aendringElement)
        {
            if(aendringDefinition.AktionType!=AktionType.Ophaev)
                throw new InvalidOperationException($"Aendring aktion for this handler should be {AktionType.Ophaev}, but is {aendringDefinition.AktionType}.");
         
        }
        public void Apply(TargetDocument targetDocument)
        {
            var targetElements = FindElementInTarget(targetDocument).ToArray();
            foreach (var targetElement in targetElements)
            {
                targetElement?.Remove();
            }
        }
    }
}