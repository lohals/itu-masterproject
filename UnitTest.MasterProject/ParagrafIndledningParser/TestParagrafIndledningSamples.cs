using Xunit;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.ParagrafIndledningParser
{
    public class TestParagrafIndledningSamples
    {
        [Theory]
        [InlineData("I lov om jordbrugets anvendelse af gødning og om plantedække" +
                    ", jf. lovbekendtgørelse nr. 388 af 27. april 2016, foretages følgende ændringer:", 388, 2016, "lov om jordbrugets anvendelse af gødning og om plantedække")]
        [InlineData("I økologilov, lov nr. 463 af 17. juni 2008, foretages følgende ændring:", 463, 2008, "økologilov")]
        //[InlineData("I kriminallov for Grønland, lov nr. 306 af 30. april 2008, som ændret ved § 1 i lov nr. 735 af 25. juni 2014 og § 4 i lov nr. 103 af 3. februar 2016, foretages følgende ændringer:", 
        //    306, 2008, "kriminallov for Grønland")]
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
