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
            ApplyStructureReplacments(targetDocument);
            ApplySubStructureReplacements(targetDocument);

        }

        private void ApplySubStructureReplacements(TargetDocument targetDocument)
        {
            var substructure = GetSupportedSubStructureTargets().ToArray();
            foreach (var saetning in substructure)
            {
                var targetStructure = GetTargetStructureForSubstructureChange(targetDocument, saetning);
            }

        }

        private void ApplyStructureReplacments(TargetDocument targetDocument)
        {
            var elementsToReplace = FindStructureElementsInTargetDocument(targetDocument, AendringDefinition.StructureTargets);
            if (!elementsToReplace?.Any() ?? false)
                return;
            var replacement = GetAendringAktionStructures();
            elementsToReplace.Skip(1).Remove();
            elementsToReplace.First().ReplaceWith(replacement);
        }
    }
}