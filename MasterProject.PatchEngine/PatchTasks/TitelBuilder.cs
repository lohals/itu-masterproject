using System.Xml.Linq;
using System.Xml.XPath;

namespace MasterProject.PatchEngine.PatchTasks
{
    public class TitelBuilder : IPatchTask
    {
        public TitelBuilder()
        {
            
        }
        public void Patch(TargetDocument targetDocument, ChangeDocument[] changeDocuments)
        {
            var firstCharTitelNode = targetDocument.Source.XPathSelectElement("//TitelGruppe/Titel/Linea/Char");
            var currentTitle = firstCharTitelNode.Value;
            if (currentTitle.StartsWith("Bekendtgørelse"))
                CreateLovbekendtgørelseTitle(currentTitle, firstCharTitelNode);
            else
                ConvertLovToLovbekendtgoerelse(currentTitle, firstCharTitelNode);
        }

        private void CreateLovbekendtgørelseTitle(string currentTitle, XElement firstCharTitelNode)
        {
            firstCharTitelNode.Value = currentTitle;
        }

        private static void ConvertLovToLovbekendtgoerelse(string currentTitle, XElement firstCharTitelNode)
        {
            var lovbekendtGoerelseTitle =
                $"Bekendtgørelse af {currentTitle[0].ToString().ToLower()}{currentTitle.Substring(1, currentTitle.Length - 1)}en";
            firstCharTitelNode.Value = lovbekendtGoerelseTitle;
        }
    }
}