using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
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
            var source = new FileInfo($"{testDataFolder}/LovH2008-463.xml");
            var changes = new[] {
                new ChangeDocument(new FileInfo($"{testDataFolder}/LovC2008-1336.xml"),new DateTime(2008,12,19),2008,1336)
            };
            var target = new FileInfo($"{testDataFolder}/LbkH2009-196.xml");

            //Act
            var patchResult = new PatchEngineFactory().Create().ApplyPatches(source, changes);
            RemoveMetaAndSchemaNodes(patchResult);

            var targetXdoc = XDocument.Load(target.FullName);
            RemoveMetaAndSchemaNodes(targetXdoc);

            string diffResult;
            var compareResult = _xmlCompare.CompareXml(targetXdoc, patchResult,out diffResult);

            if (!compareResult)
                _logger.WriteLine(diffResult);

            if (!compareResult && _launchViewer)
                _xdocDiffViewer.Launch(targetXdoc, patchResult);

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