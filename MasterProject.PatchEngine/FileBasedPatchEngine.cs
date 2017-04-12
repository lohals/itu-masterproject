using System.Xml.Linq;
using MasterProject.PatchEngine.PatchTasks;
using MasterProject.Utilities;

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
            targetDocument.Source.NormalizeWhiteSpace();
            foreach (var patchTask in _patchTask)
            {
                patchTask.Patch(targetDocument, changes);
            }
            return targetDocument.Source;
        }
    }
}
