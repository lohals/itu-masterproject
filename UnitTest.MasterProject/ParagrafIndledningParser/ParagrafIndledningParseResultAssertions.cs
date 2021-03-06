using System.Linq;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;
using Dk.Itu.Rlh.MasterProject.Parser;
using FluentAssertions;
using Xunit;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.ParagrafIndledningParser
{
    public static class ParagrafIndledningParseResultAssertions
    {
        public static void AssertParseErrors(this IParserErrorResult parserError)
        {
            Assert.All(parserError.Errors, error => Assert.Equal(string.Empty, error));
            parserError.Errors.Should().BeEmpty();
            parserError.Verbose.Should().BeEmpty();
        }

        public static void AssertParagrafIndledningResult(this ParagrafIndledningModel parseResult, int number, int year,
          string title)
        {
            parseResult.Reference.Should().NotBeNull("there should have been found a reference");
            var reference = parseResult.Reference;
            reference.DokumentReference.Should().NotBeNull("The dokument reference property should not be null.");
            reference.DokumentReference.Number.Should().Be(number);
            reference.DokumentReference.Year.Should().Be(year);
            reference.Title.Should().Be(title);
        }
    }
}