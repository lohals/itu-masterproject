using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine.Økologiloven
{
    public class FileBasedPatchEngine
    {
        private readonly IPatchTask[] _patchTask;

        public FileBasedPatchEngine(IPatchTask[] patchTask)
        {
            _patchTask = patchTask;
        }

        public XDocument ApplyPatches(FileInfo source,ChangeDocument[] changes)
        {
            var fullName = source.FullName;
            var target = XDocument.Parse(File.ReadAllText(fullName));
            foreach (var patchTask in _patchTask)
            {
                patchTask.Patch(target, changes);
            }
            return target;
        }
    }
}
