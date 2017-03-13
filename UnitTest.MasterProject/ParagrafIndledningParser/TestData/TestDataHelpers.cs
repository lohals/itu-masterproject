using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.ParagrafIndledningParser.TestData
{
    public static class TestDataHelpers
    {
        public static IEnumerable<object[]> ParseTestData(this FileInfo file)
        {
            var lines = File.ReadAllLines(file.FullName);
            return lines
                .Skip(2)
                .Select(line => line.Split(';'))
                .Select(line => line.Select(item => item.Trim('"')).ToArray())
                .Select(strings => new object[]
                {
                   strings[0],
                   int.Parse(strings[1]),
                   int.Parse(strings[2]),
                   strings[3]

                });
        }
    }
}
