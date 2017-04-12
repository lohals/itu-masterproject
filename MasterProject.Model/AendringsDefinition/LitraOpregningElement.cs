using System.Text.RegularExpressions;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class LitraOpregningElement: Element<string>
    {
        public override Element<string> CloneStrong(string nummer)
        {
            return new LitraOpregningElement() {NummerStrong = nummer};
        }

        public override string LexdaniaName => "Indentatio";

        public override Regex NummerMatch
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}