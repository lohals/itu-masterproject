using System.Text.RegularExpressions;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class Saetning : Element<int>
    {
        public override Element<int> CloneStrong(int nummer)
        {
            return new Saetning() {NummerStrong = nummer};
        }

        public override string LexdaniaName => "pkt";

        public override Regex NummerMatch
        {
            get { throw new System.NotImplementedException(); }
        }

        public override bool IsStructureElement => false;
    }
}