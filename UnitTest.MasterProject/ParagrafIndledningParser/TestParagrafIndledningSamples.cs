using Xunit;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.ParagrafIndledningParser
{
    
    public class TestParagrafIndledningSamples
    {
        [Theory]
        //Format 1
        [InlineData("I lov om jordbrugets anvendelse af gødning og om plantedække" +
                    ", jf. lovbekendtgørelse nr. 388 af 27. april 2016, foretages følgende ændringer:", 388, 2016, "lov om jordbrugets anvendelse af gødning og om plantedække")]
        [InlineData("I økologilov, lov nr. 463 af 17. juni 2008, foretages følgende ændring:", 463, 2008, "økologilov")]
        [InlineData("I kriminallov for Grønland, lov nr. 306 af 30. april 2008, som ændret ved § 1 i lov nr. 735 af 25. juni 2014 " +
                    "og § 4 i lov nr. 103 af 3. februar 2016, foretages følgende ændringer:",
            306, 2008, "kriminallov for Grønland")]
        [InlineData("I lov om arbejdsløshedsforsikring m.v., jf. lovbekendt­gørelse nr. 832 af 7. juli 2015, som ændret ved § 32 i lov nr. 994 af 30. august 2015, § 4 i lov nr. 1569 af 15. december 2015 og § 8 i lov nr. 395 af 2. maj 2016, foretages følgende ændring:",
            832, 2015, "lov om arbejdsløshedsforsikring m.v.")]

        //Format 2
        [InlineData("I lov nr. 606 af 12. juni 2013 om offentlighed i forvaltningen foretages følgende ændring:",606,2013,"")]

        //The next samples contains unicode character soft hypen in the doctype decalarion 'lovbekendtgørelse'
        [InlineData("I lov om arbejdsløshedsforsikring m.v., jf.lov­bekendt­gørelse nr. 128 af 31. januar 2017, " +
                    "som ændret ved § 1 i lov nr. 624 af 8.juni af 2016 og lov nr. 1718 af 27.december 2016, foretages følgende ændringer: ",
            128,2017,"lov om arbejdsløshedsforsikring m.v.")]
       [InlineData("I lov om arbejdsløshedsforsikring m.v., jf. lovbekendt­gørelse nr. 832 af 7. juli 2015, som ændret ved § 32 i lov nr. 994 af 30. august 2015, § 4 i lov nr. 1569 af 15. december 2015 og § 8 i lov nr. 395 af 2. maj 2016, foretages følgende ændring:",
            832, 2015, "lov om arbejdsløshedsforsikring m.v.")]
        public void TestDocumentReferenceWithNoChanges(string input,int number,int year,string title)
        {
            var parser = new global::Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning.ParagrafIndledningParser();
            var result = parser.Parse(input);

            result.ErrorResult.AssertParseErrors();

            var parseResult = result.Result;

            parseResult.AssertParagrafIndledningResult(number, year, title);
        }
    }
}
