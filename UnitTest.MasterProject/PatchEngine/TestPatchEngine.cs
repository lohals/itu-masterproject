using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;
using MasterProject.PatchEngine;
using Xunit;
using Xunit.Abstractions;
using MasterProject.Utilities;
using NUnit.Framework.Constraints;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine
{
    public class TestPatchEngine
    {
        //private bool _launchViewer = false;
        private bool _launchViewer = true;
        private readonly ITestOutputHelper _logger;
        private readonly XdocDiffViewer _xdocDiffViewer=new XdocDiffViewer();
        private readonly IXmlCompare _xmlCompare = new XmlNormalizer(new XmlCompare());
        private static readonly string PatchengineSampleconsolidationsRootFolder = "PatchEngine/SampleConsolidations";

        public TestPatchEngine(ITestOutputHelper logger)
        {
            _logger = logger;
        }

        public static IEnumerable<object[]> SampleConsolidations => new[]
        {
            //GetConsolidationSample($"{PatchengineSampleconsolidationsRootFolder}/Økologiloven/First"),
            //GetConsolidationSample($"{PatchengineSampleconsolidationsRootFolder}/Økologiloven/Second"),
            GetConsolidationSample($"{PatchengineSampleconsolidationsRootFolder}/Økologiloven/Third"),
        };
        [Theory,MemberData(nameof(SampleConsolidations))]
        public void TestOekologiLoven(TargetDocument targetDocument, ChangeDocument[] changeDocuments, FileInfo expectedConsolidation)
        {
            
            //Act
            var sut = new PatchEngineFactory().Create();

            var patchResult = sut.ApplyPatches(changeDocuments, targetDocument);

            //RemoveUnComparableElements(patchResult);

            var expectedXdoc = XDocument.Load(expectedConsolidation.FullName).NormalizeWhiteSpace();

            //RemoveUnComparableElements(expectedXdoc);

            string diffResult;
            var compareResult = _xmlCompare.CompareXml(expectedXdoc, patchResult,out diffResult);

            if (!compareResult)
                _logger.WriteLine(diffResult);

            if (!compareResult && _launchViewer)
                _xdocDiffViewer.Launch(expectedXdoc, patchResult);

            Assert.True(compareResult);

        }

        

        private static object[] GetConsolidationSample(string sampleFolder)
        {
            var testFileSet = BuildTestFileSet(sampleFolder).OrderBy(file => file.YEar).ThenBy(file => file.Number).ToArray();

            var targetFile = testFileSet.First();
            return new object[]
            {
                new TargetDocument(targetFile.FileInfo,targetFile.Type,targetFile.YEar,targetFile.Number), 
                testFileSet.Skip(1).Where(f=>f.Type==DokumentType.LovÆndring||f.Type==DokumentType.Lov).Select(file => new ChangeDocument(file.FileInfo,file.YEar,file.Number)).ToArray(),
                testFileSet.Last().FileInfo
            };
        }
        private static readonly Regex TestFilePattern= new Regex("(?<type>\\w{4})(?<year>[0-9]{4})-(?<number>[0-9]+)", RegexOptions.Compiled);
        private static IEnumerable<SampleFile> BuildTestFileSet(string testDataFolder)
        {

            return Directory.EnumerateFiles(testDataFolder)
                .Select(s => new FileInfo(s))
                .Select(info => new {info, MetaData= TestFilePattern.Match(info.Name)})
                .Select(map => new SampleFile()
                {
                    FileInfo = map.info,
                    Number = int.Parse(map.MetaData.Groups["number"].Value),
                    YEar = int.Parse(map.MetaData.Groups["year"].Value),
                    Type =  MatchFileNameDocTypeToDokumentType(map.MetaData) 

                });
        }

        private static DokumentType MatchFileNameDocTypeToDokumentType(Match match)
        {
            switch (match.Groups["type"].Value.ToLower())
            {
                case "lovh":
                    return DokumentType.Lov;
                case "lbkh":
                    return DokumentType.LovBekendtgørelse;
                case "lovc":
                    return DokumentType.LovÆndring;
                default:
                    return DokumentType.Unknown;
                               
            }
        }

      
    }

    internal class SampleFile
    {
        public DokumentType Type { get; set; }
        public int YEar { get; set; }
        public int Number { get; set; }

        public FileInfo FileInfo { get; set; }
    }
}