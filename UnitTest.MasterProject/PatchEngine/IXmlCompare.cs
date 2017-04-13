using System.Xml.Linq;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine
{
    public interface IXmlCompare
    {
        bool CompareXml(XDocument targetXdoc, XDocument patchResult,out string xmlDiff);
    }
}