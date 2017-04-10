using System.Xml.Linq;

namespace MasterProject.PatchEngine
{
    public class FileBasedPatchEngine
    {
        private readonly IPatchTask[] _patchTask;

        public FileBasedPatchEngine(IPatchTask[] patchTask)
        {
            _patchTask = patchTask;
        }

        public XDocument ApplyPatches(ChangeDocument[] changes,TargetDocument targetDocument)
        {
            foreach (var patchTask in _patchTask)
            {
                patchTask.Patch(targetDocument, changes);
            }
            return targetDocument.Source;
        }
    }
}
