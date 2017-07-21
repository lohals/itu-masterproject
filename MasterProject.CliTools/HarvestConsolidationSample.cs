using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MasterProject.PatchEngine;
using MasterProject.PatchEngine.LegalQuery;

namespace MasterProject.CliTools
{
    class HarvestConsolidationSample
    {
        public async Task HarvestCompleteSample(string legalRessource, DirectoryInfo targetDir)
        {
            var qs = new RdfBasedLegalDocumentLoader().Load(legalRessource);
            var sampleFileName = GetLegalResourceFilename(legalRessource, qs);

            var client = new WebClient();
            await DownloadFileAsync(targetDir, client, sampleFileName, new Uri($"{legalRessource}/xml"));

            foreach (var changeDocument in qs.GetChangeDocuments())
            {
                var ressource = changeDocument.LegalRessource.ToString();
                var rdfQuerySource = GetRdfQuerySource(ressource);
                var fileName = GetLegalResourceFilename(ressource,rdfQuerySource);
                await DownloadFileAsync(targetDir, client, fileName, changeDocument.LegalExpressionXmlFormatUri);
            }
            var targetConsolidation = qs.GetLaterConsolidationUri();
            await DownloadFileAsync(targetDir, client,
                GetLegalResourceFilename(targetConsolidation, GetRdfQuerySource(targetConsolidation)),new Uri($"{targetConsolidation}/xml"));
        }

        private static RdfQuerySource GetRdfQuerySource(string ressource)
        {
            return new RdfBasedLegalDocumentLoader().Load(ressource);
        }

        private static string GetLegalResourceFilename(string legalRessource, RdfQuerySource qs)
        {
            var docType = qs.GetDocTypeShortName();
            var components = legalRessource.Split('/');
            var number = int.Parse(components.Last());
            var year = int.Parse(components[components.Length - 2]);
            var startDocFileName = GetFileName(docType, year, number);
            return startDocFileName;
        }

        private static async Task DownloadFileAsync(DirectoryInfo targetDir, WebClient client, string startDocFileName, Uri address)
        {
            await client.DownloadFileTaskAsync(address,Path.Combine(targetDir.FullName, startDocFileName));
        }

        private static string GetFileName(string docType, int year, int number)
        {
            return $"{docType}{year}-{number}.xml";
        }
    }

}
