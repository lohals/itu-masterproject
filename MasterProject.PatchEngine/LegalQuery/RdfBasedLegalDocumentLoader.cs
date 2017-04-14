using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;

namespace MasterProject.PatchEngine.LegalQuery
{
    public class RdfBasedLegalDocumentLoader
    {
        public RdfQuerySource Load(string legalRessource)
        {
            var g = new Graph();
            UriLoader.Load(g, new Uri($"{legalRessource}.rdf"));
            return new RdfQuerySource(g,legalRessource);
        }
    }
}
