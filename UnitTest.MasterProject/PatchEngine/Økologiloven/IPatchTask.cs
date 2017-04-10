using System.Xml.Linq;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine.Økologiloven
{
    public interface IPatchTask
    {
        void Patch(XDocument source, ChangeDocument[] changes);
    }

    class ApplyAendringAktioner : IPatchTask
    {
        public ApplyAendringAktioner()
        {
            
        }
        public void Patch(XDocument source, ChangeDocument[] changes)
        {
            
        }
    }
}