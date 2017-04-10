using System.Xml.Linq;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine.Økologiloven
{
    public class PatchHymne : IPatchTask
    {
        public void Patch(XDocument source,ChangeDocument[] changeDocuments)
        {
            source.Root.Descendants("Hymne").Remove();


        }
    }
}