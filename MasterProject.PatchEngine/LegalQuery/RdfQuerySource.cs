using System;
using System.Collections.Generic;
using System.Linq;
using Dk.Itu.Rlh.MasterProject.Model;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;
using VDS.RDF;
using VDS.RDF.Nodes;

namespace MasterProject.PatchEngine.LegalQuery
{
    public class RdfQuerySource
    {
        private readonly Graph _graph;
        private readonly string _root;

        public RdfQuerySource(Graph graph,string root)
        {
            _graph = graph;
            _root = root;
        }
        public IEnumerable<ChangeDocument> GetChangeDocuments()
        {
            var chanagedByTriples = GetPropertyTriple("eli:changed_by");

            var changedByDocInfos = chanagedByTriples.Select(triple => new ChangeDocument(new Uri($"{triple.Object.AsValuedNode().AsString()}"), 1, 1));
            return changedByDocInfos;
        }

        public DokumentType GetDocType()
        {
            var rawDocTypeShortName = GetDocTypeShortName();
            return DokumentTypeHelpers.MapShortNameToDokumentType(rawDocTypeShortName);
        }

        public string GetDocTypeShortName()
        {
            var triple = GetPropertyTriple("eli:type_document");
            var rawDocTypeShortName = triple.First().Object.AsValuedNode().AsString().Split('#')[1];
            return rawDocTypeShortName;
        }

        private IEnumerable<Triple> GetPropertyTriple(string eliPropertyName)
        {
            return _graph.GetTriplesWithSubjectPredicate(_graph.CreateUriNode(new Uri(_root)),_graph.CreateUriNode(eliPropertyName));
        }

        public string GetLaterConsolidationUri()
        {
            var triple = GetPropertyTriple("eli:consolidated_by");
            return triple.First().Object.AsValuedNode().AsString();
        }
    }
}