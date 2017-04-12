using System;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace MasterProject.PatchEngine.AendringHandlers
{
    public class AendringAktionHandlerFactory
    {
        public IAendringAktionHandler Create(AendringDefinition aendringDefinition, XElement changeItem1)
        {
            switch (aendringDefinition.AktionType)
            {
                case AktionType.IndsaetEfter:
                    return new IndsaetEfterHandler(aendringDefinition,changeItem1);
                case AktionType.IndsaetFoer:
                case AktionType.Manuel:
                case AktionType.Ophaev:
                    return new OphaevAendringHandler(aendringDefinition, changeItem1);
                case AktionType.Erstat:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}