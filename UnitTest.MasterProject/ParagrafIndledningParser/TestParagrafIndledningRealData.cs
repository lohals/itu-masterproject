using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnitTest.Dk.Itu.Rlh.MasterProject.ParagrafIndledningParser.TestData;
using Xunit;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.ParagrafIndledningParser
{
    public class TestParagrafIndledningRealData
    {
        private static readonly string TestDataDir = "ParagrafIndledningParser/TestData";

        public static IEnumerable<object[]> TestData_Format1
        {
            get
            {
                var fileNames = new[]
                {
                    "2017-format1.csv",
                    "2016-format1.csv",
                    "2015-format1.csv",
                    "2014-format1.csv",
                    "2013-format1.csv",
                    "2012-format1.csv",
                    "2011-format1.csv",
                    "2010-format1.csv",
                };
                return
                     ParseFiles(fileNames);
            }
        }

        private static IEnumerable<object[]> ParseFiles(string[] fileNames)
        {
            return fileNames
                .Select(filename => new FileInfo($"{TestDataDir}/{filename}"))
                .SelectMany(file => file.ParseTestData());
        }

        public static IEnumerable<object[]> TestData_Format2
        {
            get
            {
                var fileNames = new[]
                {
                    "2017-format2.csv",
                    "2016-format2.csv",
                    "2015-format2.csv",
                    "2014-format2.csv",
                    "2013-format2.csv",
                    "2012-format2.csv",
                    "2011-format2.csv",
                    "2010-format2.csv",
                };
                return
                     ParseFiles(fileNames);
            }
        }



        [Theory(Skip = "not ready")]
        [MemberData(nameof(TestData_Format1))]
        public void TestParagrafIndledning_Format1Data(string input, int number, int year, string title)
        {
            AssertResult(input, number, year, title);
        }

        [Theory(Skip = "not ready")]
        [MemberData(nameof(TestData_Format2))]
        public void TestParagrafIndledning_Format2Data(string input, int number, int year, string title)
        {
            AssertResult(input, number, year, title);
        }
        private static void AssertResult(string input, int number, int year, string title)
        {
            var parser = new global::Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning.ParagrafIndledningParser();
            var result = parser.Parse(input);

            result.ErrorResult.AssertParseErrors();

            var parseResult = result.Result;

            parseResult.AssertParagrafIndledningResult(number, year, title);
        }
    }

}
