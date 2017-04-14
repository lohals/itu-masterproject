using System;
using System.IO;
using System.Xml.Linq;
using VDS.RDF.Query.Expressions.Functions.XPath.DateTime;

namespace MasterProject.PatchEngine
{
    public class ChangeDocument
    {
        public Uri LegalRessource { get; }
        public int Year { get; }
        public int Number { get; }

        private readonly FileInfo _fileInfo;

        public ChangeDocument(Uri legalRessource,int year, int number)
        {
            LegalRessource = legalRessource;
            Year = year;
            Number = number;
        }
        public ChangeDocument(FileInfo fileInfo, int year, int number)
        {
            Number = number;
            _fileInfo = fileInfo;
            Year = year;
        }
        public XDocument XDoc => _fileInfo==null
            ?XDocument.Load(LegalExpressionXmlFormatUri.ToString())
            :XDocument.Load(_fileInfo.FullName);

        public Uri LegalExpressionXmlFormatUri => LegalRessource==null?null:new Uri($"{LegalRessource}/xml");
    }
}