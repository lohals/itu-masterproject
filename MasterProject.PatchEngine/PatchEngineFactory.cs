using System.Linq;
using MasterProject.PatchEngine.PatchTasks;

namespace MasterProject.PatchEngine
{
    public class PatchEngineFactory
    {
        private static IPatchTask[] _patchTask;

        public PatchEngineFactory()
        {
            _patchTask=new IPatchTask[]
            {
                new SchemaPatch("http://www.retsinformation.dk/offentlig/xml/schemas/2011/03/21/LBKH.Retsinfo.LexDania_2.1.xsd"),
                new TitelBuilder(),
                new PatchHymne(), 
                new IndledningBuilder(),
                new ApplyAendringAktioner(), 
                new SeglPatchTask()
            };
        }
        public FileBasedPatchEngine Create()
        {
            return new FileBasedPatchEngine(_patchTask);
        }
    }

    public class SeglPatchTask : IPatchTask
    {
        public void Patch(TargetDocument targetDocument, ChangeDocument[] changes)
        {
            targetDocument.Source.Descendants("Segl").FirstOrDefault()?.Remove();
        }
    }
}