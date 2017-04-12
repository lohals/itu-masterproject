using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;
using MasterProject.PatchEngine;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine
{
    public class TestPatchEngine
    {
        private readonly ITestOutputHelper _logger;
        private bool _launchViewer = false;
        private readonly XdocDiffViewer _xdocDiffViewer=new XdocDiffViewer();
        private readonly XmlCompare _xmlCompare = new XmlCompare();
        private static readonly string PatchengineSampleconsolidationsRootFolder = "PatchEngine/SampleConsolidations";

        public TestPatchEngine(ITestOutputHelper logger)
        {
            _logger = logger;
        }


        [Theory,MemberData(nameof(SampleConsolidations))]
        public void TestOekologiLoven(TargetDocument targetDocument, ChangeDocument[] changeDocuments, FileInfo expectedConsolidation)
        {

            //Act
            var sut = new PatchEngineFactory().Create();

            var patchResult = sut.ApplyPatches(changeDocuments, targetDocument);

            RemoveMetaAndSchemaNodes(patchResult);

            var expectedXdoc = XDocument.Load(expectedConsolidation.FullName);
            RemoveMetaAndSchemaNodes(expectedXdoc);

            string diffResult;
            var compareResult = _xmlCompare.CompareXml(expectedXdoc, patchResult,out diffResult);

            if (!compareResult)
                _logger.WriteLine(diffResult);

            if (!compareResult && _launchViewer)
                _xdocDiffViewer.Launch(expectedXdoc, patchResult);

            Assert.True(compareResult);

        }

        public static IEnumerable<object[]> SampleConsolidations => new[]
        {
            //GetFirstOekologiLovenConsolidation(),
            GetSecondOekologiLovenConsolidation()
        };

        private static object[] GetFirstOekologiLovenConsolidation()
        {
            var testDataFolder = $"{PatchengineSampleconsolidationsRootFolder}/Økologiloven/First";
            return new object[]
            {
                new TargetDocument(new FileInfo($"{testDataFolder}/LovH2008-463.xml"), DokumentType.Lov, 2008, 463),
                new[] {
                    new ChangeDocument(new FileInfo($"{testDataFolder}/LovC2008-1336.xml"),new DateTime(2008,12,19),2008,1336)
                },
                new FileInfo($"{testDataFolder}/LbkH2009-196.xml")
            };
        }
        private static object[] GetSecondOekologiLovenConsolidation()
        {
            var testDataFolder = $"{PatchengineSampleconsolidationsRootFolder}/Økologiloven/Second";
            return new object[]
            {
                new TargetDocument(new FileInfo($"{testDataFolder}/LbkH2009-196.xml"), DokumentType.Lov, 2008, 463),
                new[] {
                    new ChangeDocument(new FileInfo($"{testDataFolder}/LovC2011-341.xml"),new DateTime(2008,12,19),2008,1336)
                },
                new FileInfo($"{testDataFolder}/LbkH2011-416.xml")
            };
        }
        private static void RemoveMetaAndSchemaNodes(XDocument patchResult)
        {
            patchResult.Root.Descendants("Meta").Remove();
            patchResult.Root.Attributes("SchemaLocation").Remove();
            patchResult.Root.Attributes("id").Remove();
        }
    }
}