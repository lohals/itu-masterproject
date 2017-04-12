using System.Text.RegularExpressions;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class Paragraf : Element<string>
    {

        public override Element<string> CloneStrong(string nummer)
        {
           return new Paragraf() {NummerStrong = nummer };
        }

        public override string LexdaniaName => "Paragraf";

        public override Regex NummerMatch => new Regex($@"§\s{NummerStrong}", RegexOptions.IgnoreCase);

    }
}