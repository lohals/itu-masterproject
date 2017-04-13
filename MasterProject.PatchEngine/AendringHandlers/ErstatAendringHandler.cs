using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace MasterProject.PatchEngine.AendringHandlers
{
    public class ErstatAendringHandler : BaseAendringAktionHandler, IAendringAktionHandler
    {
        public ErstatAendringHandler(AendringDefinition aendringDefinition, XElement aendring) : base(aendringDefinition,aendring)
        {
            
        }

        public void Apply(TargetDocument targetDocument)
        {
            var elementsToReplace = FindStructureElementsInTargetDocument(targetDocument, AendringDefinition.StructureTargets);
            var replacement = GetAendringAktionStructures();
            elementsToReplace.Skip(1).Remove();
            elementsToReplace.First().ReplaceWith(replacement);

        }
    }
}