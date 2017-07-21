using System.Linq;
using System.Xml.Linq;

namespace MasterProject.PatchEngine.PatchTasks
{
    public class IndledningBuilder : IPatchTask
    {
        //Herved bekendtgøres økologiloven, jf. lovbekendtgørelse nr. 196 af 12. marts 2009, med de ændringer, der følger af § 7 i lov nr. 341 af 27. april 2011.
        //Herved bekendtgøres økologiloven, lov nr. 463 af 17. juni 2008, med de ændringer, der følger af § 70 i lov nr. 1336 af 19. december 2008.
        public void Patch(TargetDocument targetDocument, ChangeDocument[] changes)
        {
            var name= "økologiloven";
            var docDescription= "lov nr. 463 af 17. juni 2008";
            var aendringsParagrafReference= "§ 70";
            var changeDocDescription= "lov nr. 1336 af 19. december 2008";
            var template = $"Herved bekendtgøres {name}, {docDescription}, med de ændringer, der følger af {aendringsParagrafReference} i {changeDocDescription}.";
            
            var dokumentIndhold = targetDocument.Source.Descendants("DokumentIndhold").FirstOrDefault();
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