using System;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace MasterProject.PatchEngine.AendringHandlers
{
    public class AendringAktionHandlerFactory
    {
        public IAendringAktionHandler Create(AendringDefinition aendringDefinition, XElement aendring)
        {
            switch (aendringDefinition.AktionType)
            {
                case AktionType.IndsaetEfter:
                    return new IndsaetEfterHandler(aendringDefinition, aendring);
      
                case AktionType.Ophaev:
                    return new OphaevAendringHandler(aendringDefinition, aendring);
                case AktionType.Erstat:
                    return new ErstatAendringHandler(aendringDefinition, aendring);
                case AktionType.IndsaetFoer:
                    return new IndsaetFoerHandler(aendringDefinition, aendring);
                case AktionType.Manuel:
                    return new NoopManuelHandler();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}