namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class ParentElementContext : Element
    {

        public override object Nummer
        {
            get { return null; }
        }

        public override Element Clone(object nummer)
        {
            return new ParentElementContext();
        }
    }
}