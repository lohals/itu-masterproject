using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.XmlDiffPatch;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine.Økologiloven
{
    public class XmlCompare
    {
        public XmlCompare()
        {
        }

        public bool CompareXml(XDocument targetXdoc, XDocument patchResult,out string xmlDiff)
        {
            bool compareResult;
            using (var expectedXml = targetXdoc.CreateReader())
            using (var inMemDiff = new MemoryStream())
            using (var diffgram = XmlWriter.Create(inMemDiff, new XmlWriterSettings() {Indent = true}))
            {
                compareResult = GetDiffTool().Compare(expectedXml, patchResult.CreateReader(), diffgram);
                diffgram.Flush();
                inMemDiff.Position = 0;
                xmlDiff = Encoding.UTF8.GetString(inMemDiff.ToArray());
            }
            return compareResult;
        }


        private static XmlDiff GetDiffTool()
        {
            var diff = new Microsoft.XmlDiffPatch.XmlDiff();
            diff.IgnoreWhitespace = true;
            diff.IgnoreComments = true;
            diff.IgnoreXmlDecl = true;
            diff.IgnoreDtd = true;
            //diff.IgnoreChildOrder = true;
            diff.IgnorePrefixes = true;
            return diff;
        }
    }
}