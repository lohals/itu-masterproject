using System;
using System.Text.RegularExpressions;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class Stk : Element<int>
    {

        public override Element<int> CloneStrong(int nummer)
        {
            return new Stk() {NummerStrong = nummer};
        }

        public override string LexdaniaName => "Stk";
        public override Regex NummerMatch => new Regex($@"Stk\.\s{NummerStrong}",RegexOptions.IgnoreCase);
    }
}
