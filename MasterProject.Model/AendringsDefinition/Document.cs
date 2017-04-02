namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class DokumentElement : Element
    {
        public override object Nummer =>this;
        public override Element Clone(object nummer=null)
        {
            return new DokumentElement();
        }
    }
}