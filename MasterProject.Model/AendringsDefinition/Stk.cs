namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class Stk : Element<int>
    {

        public override Element<int> CloneStrong(int nummer)
        {
            return new Stk() {NummerStrong = nummer};
        }
    }
}
