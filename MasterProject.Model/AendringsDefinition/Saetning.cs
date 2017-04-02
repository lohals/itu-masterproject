namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class Saetning : Element<int>
    {
        public override Element<int> CloneStrong(int nummer)
        {
            return new Saetning() {NummerStrong = nummer};
        }
    }
}