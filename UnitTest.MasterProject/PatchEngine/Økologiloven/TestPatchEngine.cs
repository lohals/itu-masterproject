using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;
using Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning;
using MasterProject.PatchEngine;
using Microsoft.XmlDiffPatch;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine.Økologiloven
{
    public class TestPatchEngine
    {
        private readonly ITestOutputHelper _logger;
        private bool _launchViewer = false;
        private readonly XdocDiffViewer _xdocDiffViewer=new XdocDiffViewer();
        private readonly XmlCompare _xmlCompare = new XmlCompare();

        public TestPatchEngine(ITestOutputHelper logger)
        {
            _logger = logger;
        }
        [Fact]
        public void TestOekologiLoven()
        {
            var testDataFolder = "PatchEngine/Økologiloven/First";
            var targetDocument = new TargetDocument(new FileInfo($"{testDataFolder}/LovH2008-463.xml"), DokumentType.Lov,2008,463);
            var changes = new[] {
                new ChangeDocument(new FileInfo($"{testDataFolder}/LovC2008-1336.xml"),new DateTime(2008,12,19),2008,1336)
            };
            var target = new FileInfo($"{testDataFolder}/LbkH2009-196.xml");

            //Act
            var sut = new PatchEngineFactory().Create();

            var patchResult = sut.ApplyPatches(changes, targetDocument);

            RemoveMetaAndSchemaNodes(patchResult);

            var expectedXdoc = XDocument.Load(target.FullName);
            RemoveMetaAndSchemaNodes(expectedXdoc);

            string diffResult;
            var compareResult = _xmlCompare.CompareXml(expectedXdoc, patchResult,out diffResult);

            if (!compareResult)
                _logger.WriteLine(diffResult);

            if (!compareResult && _launchViewer)
                _xdocDiffViewer.Launch(expectedXdoc, patchResult);

            Assert.True(compareResult);

        }

        private static void RemoveMetaAndSchemaNodes(XDocument patchResult)
        {
            patchResult.Root.Descendants("Meta").Remove();
            patchResult.Root.Attributes("SchemaLocation").Remove();
            patchResult.Root.Attributes("id").Remove();
        }
    }
}