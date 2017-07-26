using Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.AendringsDefinitionParser
{
    [Trait("Category", "Tool")]
    public class TestRealDataParsing
    {
        private readonly ITestOutputHelper _logger;

        public TestRealDataParsing(ITestOutputHelper logger)
        {
            _logger = logger;
        }

        //[Theory(Skip = "not ready")]
        [Theory]
        [InlineData("2017")]
        [InlineData("2016")]
        [InlineData("2015")]
        [InlineData("2014")]
        [InlineData("2013")]
        public void AllCST_AendringsDefinitioner(int year)
        {
            var file = new FileInfo($"AendringsDefinitionParser/RealTestData/{year}-cst-aendringsdefinitioner.txt");
            var definitions = File.ReadAllLines(file.FullName);
            var sut = new AendringDefintionParser();
            var errors = definitions
                .Select(line => line.Split(new[] { ';' }))
                .Select((def, no) => new
                {
                    LineNo = no,
                    SourceDok = def[0],
                    Text = def[1],
                    Result = sut.Parse(def[1])
                })
                .Where(r => r.Result.ErrorResult.Errors.Any())
                .ToArray();


            _logger.WriteLine($"COMPLETE: Parsed {definitions.Length} defintions from year {year} and found {errors.Length} errors ({((double)(definitions.Length - errors.Length) / definitions.Length) * 100:0.#}% was parsed without errors):");

            foreach (var error in errors)
            {
                _logger.WriteLine("");
                _logger.WriteLine($"LineNo. {error.LineNo} - [{error.Text}]");
                foreach (var errorResultError in error.Result.ErrorResult.Errors)
                {
                    _logger.WriteLine($"   From source document {error.SourceDok}");
                    _logger.WriteLine($"   {errorResultError}");

                }
            }



            Assert.Equal(definitions.Length, definitions.Length - errors.Count());
        }
    }
}
