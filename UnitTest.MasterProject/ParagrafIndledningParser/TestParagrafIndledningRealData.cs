using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.ParagrafIndledningParser
{
    public class TestParagrafIndledningRealData
    {

        public static IEnumerable<object[]> TestData2017_Format1
        {
            get
            {
                return
                    ParseTestData(new FileInfo("ParagrafIndledningParser/TestData/2017-format1.csv"));
            }
        }

        private static IEnumerable<object[]> ParseTestData(FileInfo file)
        {
            var lines = File.ReadAllLines(file.FullName);
            return lines
                .Skip(2)
                .Select(line => line.Split(';'))
                .Select(line =>line.Select(item=>item.Trim('"')).ToArray())
                .Select(strings => new object[]
                {
                   strings[0],
                   int.Parse(strings[2]),
                   int.Parse(strings[1]),
                   strings[3]

                });
        }

        
        [Theory(Skip="Grammar not ready")]
        [MemberData(nameof(TestData2017_Format1))]
        public void TestParagrafIndledning_Format1Data(string input, int number, int year, string title)
        {
            var parser = new global::Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning.ParagrafIndledningParser();
            var result = parser.Parse(input);

            result.ErrorResult.AssertParseErrors();

            var parseResult = result.Result;

            parseResult.AssertParagrafIndledningResult(number, year, title);

        }
    }
    
}
