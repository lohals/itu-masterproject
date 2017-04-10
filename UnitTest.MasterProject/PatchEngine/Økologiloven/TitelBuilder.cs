using System.Xml.Linq;
using System.Xml.XPath;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine.Økologiloven
{
    public class TitelBuilder : IPatchTask
    {
        public void Patch(XDocument source, ChangeDocument[] changeDocuments)
        {
            var firstCharTitelNode = source.XPathSelectElement("//TitelGruppe/Titel/Linea/Char");
            var currentTitle = firstCharTitelNode.Value;
            var lovbekendtGoerelseTitle =
                $"Bekendtgørelse af {currentTitle[0].ToString().ToLower()}{currentTitle.Substring(1, currentTitle.Length - 1)}en";
            firstCharTitelNode.Value = lovbekendtGoerelseTitle;
        }
    }
}