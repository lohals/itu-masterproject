using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning;
using FluentAssertions;
using Dk.Itu.Rlh.MasterProject.Parser;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.ParagrafIndledning
{
    public class TestParagrafIndledning
    {
        [Theory]
        //[InlineData("I kologilov, lov nr. 463 af 17. juni 2008, foretages følgende ændring:", 463, 2008, "økologilov")]
        [InlineData("I økologilov, lov nr. 463 af 17. juni 2008:", 463, 2008, "økologilov")]
        //[InlineData("I 463:", 463, 2008, "økologilov")]
        public void TestSingleDocumentReference(string input,int number,int year,string title)
        {
            var parser = new ParagrafIndledningParser();
            var result = parser.Parse(input);
            AssertError(result);

            var parseResult = result.Result;

            parseResult.References.Count().Should().Be(1);
            var reference = parseResult.References.First();
            reference.DokumentReference.Number.Should().Be(number);
            reference.DokumentReference.Year.Should().Be(year);
            reference.Title.Should().Be(title);
        }

        private void AssertError(ParseResult<ParagrafIndledningModel> result)
        {
            var parseErrrors = result.ErrorResult;
            Assert.All(parseErrrors.Verbose, error => Assert.Equal(string.Empty, error));
            parseErrrors.Errors.Should().BeEmpty();
        }
        /*
I lov om påligningen af indkomstskat (ligningsloven), jf. lovbekendtgørelse nr. 1365 af 29. november 2010, som ændret senest ved § 7 i lov nr. 254 af 30. marts 2011, foretages følgende ændring:*/
    }
}
