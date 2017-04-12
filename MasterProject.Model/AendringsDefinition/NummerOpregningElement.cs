using System.Text.RegularExpressions;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class NummerOpregningElement : Element<int>
    {
       

        public override Element<int> CloneStrong(int nummer)
        {
           return new NummerOpregningElement() {NummerStrong = nummer};
        }

        public override string LexdaniaName => "Indentatio";

        public override Regex NummerMatch
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}