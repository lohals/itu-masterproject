using System.Linq;
using System.Xml.Linq;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine.Økologiloven
{
    public class IndledningBuilder : IPatchTask
    {
        public void Patch(XDocument source,ChangeDocument[] changes)
        {
            var name= "økologiloven";
            var docDescription= "lov nr. 463 af 17. juni 2008";
            var aendringsParagrafReference= "§ 70";
            var changeDocDescription= "lov nr. 1336 af 19. december 2008";
            var template = $"Herved bekendtgøres {name}, {docDescription}, med de ændringer, der følger af {aendringsParagrafReference} i {changeDocDescription}.";
            
            var dokumentIndhold = source.Descendants("DokumentIndhold").FirstOrDefault();
            dokumentIndhold.Descendants("Indledning").Remove();
            AddNewIndledning(template, dokumentIndhold);
            //Herved bekendtgøres økologiloven, lov nr. 463 af 17. juni 2008, med de ændringer, der følger af § 70 i lov nr. 1336 af 19. december 2008.</Char>
        }

        private static void AddNewIndledning(string template, XElement dokumentIndhold)
        {
            var xChar = new XElement("Char", template);
            var xLinea = new XElement("Linea", xChar);
            var xExitus = new XElement("Exitus", xLinea);
            dokumentIndhold.AddFirst(new XElement("Indledning", xExitus));
        }
    }
}