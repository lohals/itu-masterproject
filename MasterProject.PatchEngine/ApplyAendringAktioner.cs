using System.Linq;

namespace MasterProject.PatchEngine
{
    class ApplyAendringAktioner : IPatchTask
    {
        public ApplyAendringAktioner()
        {
            
        }
        public void Patch(TargetDocument targetDocument, ChangeDocument[] changes)
        {
            var chronologicalOrder = changes.OrderBy(document => document.UnderskriftDato);
            

        }
    }
}