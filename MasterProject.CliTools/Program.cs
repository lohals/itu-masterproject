using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MasterProject.PatchEngine;
using MasterProject.PatchEngine.LegalQuery;

namespace MasterProject.CliTools
{
    class Program
    {
        public static void Main(string[] args)
        {
            var harvester= new HarvestConsolidationSample();
            harvester
                .HarvestCompleteSample("http://www.retsinformation.dk/eli/lta/2015/1255", 
                new DirectoryInfo(@"D:\Projects\itu-masterproject\UnitTest.MasterProject\PatchEngine\SampleConsolidations\Retsplejeloven\First"))
                .Wait();
        }
    }
}
