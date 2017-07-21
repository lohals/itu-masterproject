using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace MasterProject.PatchEngine.AendringHandlers
{
    public class IndsaetFoerHandler : BaseAendringAktionHandler,IAendringAktionHandler
    {
        public IndsaetFoerHandler(AendringDefinition aendringDefinition, XElement aendring) : base(aendringDefinition,aendring)
        {
        }

        public void Apply(TargetDocument targetDocument)
        {
            

        }
    }
}