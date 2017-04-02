namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class LitraOpregningElement: Element<string>
    {
        public override Element<string> CloneStrong(string nummer)
        {
            return new LitraOpregningElement() {NummerStrong = nummer};
        }
    }
}