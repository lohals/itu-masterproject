using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;
using MasterProject.PatchEngine;
using MasterProject.PatchEngine.LegalQuery;
using Xunit;
using Xunit.Abstractions;
using MasterProject.Utilities;
using NUnit.Framework.Constraints;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine
{
    public class TestPatchEngine
    {
        private bool _launchViewer = false;
        //private bool _launchViewer = true;
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
            GetConsolidationSample($"{PatchengineSampleconsolidationsRootFolder}/Økologiloven/First"),
            GetConsolidationSample($"{PatchengineSampleconsolidationsRootFolder}/Økologiloven/Second"),
            GetConsolidationSample($"{PatchengineSampleconsolidationsRootFolder}/Økologiloven/Third"),
            //GetConsolidationSample($"{PatchengineSampleconsolidationsRootFolder}/Retsplejeloven/First"),
        };

        private static object[] GetWebBasedSample(string legalRessource)
        {
            var qs = new RdfBasedLegalDocumentLoader().Load(legalRessource);
            var docType = qs.GetDocType();
            var components = legalRessource.Split('/');
            var number = int.Parse(components.Last());
            var year = int.Parse(components[components.Length - 2]);
            var target = new TargetDocument(new Uri($"{legalRessource}/xml"), docType,year,number);
            return new object[]{ target, qs.GetChangeDocuments(),XDocument.Load($"{qs.GetLaterConsolidationUri()}/xml")};
        }

        [Theory,MemberData(nameof(SampleConsolidations))]
        public void TestOekologiLoven(TargetDocument targetDocument, ChangeDocument[] changeDocuments, XDocument expectedXdoc)
        {
            
            var sut = new PatchEngineFactory().Create();

            //Act
            var patchResult = sut.ApplyPatches(changeDocuments, targetDocument);

            

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
                XDocument.Load(testFileSet.Last().FileInfo.FullName)
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
            var value = match.Groups["type"].Value;
            return DokumentTypeHelpers.MapShortNameToDokumentType(value);
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