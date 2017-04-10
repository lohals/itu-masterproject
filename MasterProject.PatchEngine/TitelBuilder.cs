using System.Xml.XPath;

namespace MasterProject.PatchEngine
{
    public class TitelBuilder : IPatchTask
    {
        public void Patch(TargetDocument targetDocument, ChangeDocument[] changeDocuments)
        {
            var firstCharTitelNode = targetDocument.Source.XPathSelectElement("//TitelGruppe/Titel/Linea/Char");
            var currentTitle = firstCharTitelNode.Value;
            var lovbekendtGoerelseTitle =
                $"Bekendtgørelse af {currentTitle[0].ToString().ToLower()}{currentTitle.Substring(1, currentTitle.Length - 1)}en";
            firstCharTitelNode.Value = lovbekendtGoerelseTitle;
        }
    }
}