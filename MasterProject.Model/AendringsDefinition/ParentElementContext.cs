using System.Text.RegularExpressions;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class ParentElementContext : Element
    {

        public override object Nummer
        {
            get { return null; }
        }

        public override string LexdaniaName
        {
            get { throw new System.NotImplementedException(); }
        }

        public override Regex NummerMatch
        {
            get { throw new System.NotImplementedException(); }
        }

        public override Element Clone(object nummer)
        {
            return new ParentElementContext();
        }
    }
}