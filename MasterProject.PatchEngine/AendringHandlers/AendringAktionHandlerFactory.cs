using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace MasterProject.PatchEngine.AendringHandlers
{
    public class AendringAktionHandlerFactory
    {
        public OphaevAendringHandler Create(AendringDefinition aendringDefinition, XElement changeItem1)
        {
            return new OphaevAendringHandler(aendringDefinition,changeItem1);
        }
    }
}