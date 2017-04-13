using System.Text.RegularExpressions;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class DokumentElement : Element
    {
        public override object Nummer =>this;

        public override string LexdaniaName
        {
            get { throw new System.NotImplementedException(); }
        }

        public override Regex NummerMatch => null;

        public override Element Clone(object nummer=null)
        {
            return new DokumentElement();
        }

        public override bool IsStructureElement => false;
    }
}