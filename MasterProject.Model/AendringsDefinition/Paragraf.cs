namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class Paragraf : Element<string>
    {

        public override Element<string> CloneStrong(string nummer)
        {
           return new Paragraf() {NummerStrong = nummer };
        }
    }
}