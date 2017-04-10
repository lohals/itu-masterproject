using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine.Økologiloven
{
    public class XdocDiffViewer
    {


        public void Launch(XDocument targetXdoc, XDocument patchResult)
        {
            File.WriteAllText("expected.xml", targetXdoc.ToString());
            File.WriteAllText("result.xml", patchResult.ToString());
            LaunchViewer(new FileInfo("result.xml").FullName, new FileInfo("expected.xml").FullName);
        }

        public void LaunchViewer(string inputFilepath, string referenceFilepath)
        {
            var path = new FileInfo(Environment.GetEnvironmentVariable("VS140COMNTOOLS") + "/../IDE/vsdiffmerge.exe");


            var startInfo = new ProcessStartInfo()
            {
                FileName = path.FullName,
                Arguments = $"{new FileInfo(referenceFilepath).FullName} {new FileInfo(inputFilepath).FullName} {new FileInfo(inputFilepath).FullName} {new FileInfo(inputFilepath).FullName} /m",
                UseShellExecute = true,
                CreateNoWindow = true,

            };
            var process = (new Process() { StartInfo = startInfo });
            process.Start();
            process.WaitForExit();
        }
    }
}