using System.Xml.Linq;

namespace MasterProject.PatchEngine.PatchTasks
{
    public class PatchHymne : IPatchTask
    {
        public void Patch(TargetDocument targetDocument, ChangeDocument[] changeDocuments)
        {
            targetDocument.Source.Root.Descendants("Hymne").Remove();


        }
    }
}