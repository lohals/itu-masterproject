using System;
using System.Linq;
using System.Xml.Linq;
using MasterProject.Utilities;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine
{
    public class XmlNormalizer : IXmlCompare
    {
        private readonly XmlCompare _comparer;

        public XmlNormalizer(XmlCompare comparer)
        {
            _comparer = comparer;
        }
        public bool CompareXml(XDocument targetXdoc, XDocument patchResult, out string xmlDiff)
        {
            targetXdoc.NormalizeWhiteSpace();
            RemoveUnComparableElements(targetXdoc);
            RemoveUnComparableElements(patchResult);

            NormalizeWhiteSpace(targetXdoc, patchResult);
            return _comparer.CompareXml(targetXdoc, patchResult, out xmlDiff);
        }

        private static void NormalizeWhiteSpace(XDocument targetXdoc, XDocument patchResult)
        {
            foreach (var descendantExitus in targetXdoc.Descendants("Exitus").Concat(patchResult.Descendants("Exitus")))
            {
                var firstLinea = descendantExitus.Element("Linea");
                if (firstLinea == null) //Maybe a list
                    continue;


                var normalizedChars = descendantExitus.Value.Split('.')
                    .Select(content => new XElement("Linea", new XElement("Char", $"{content}.")))
                    .ToArray();
                descendantExitus.Descendants("Linea").Remove();
                descendantExitus.Add(normalizedChars);
            }
        }

        private static void RemoveUnComparableElements(XDocument xDoc)
        {
            xDoc.Root.Descendants("Meta").Remove();
            xDoc.Root.Attributes("SchemaLocation").Remove();
            xDoc.Descendants().Select(element => element.Attribute("id")).Remove();
            xDoc.Descendants().Select(element => element.Attribute("localId")).Remove();
            xDoc.Root.Descendants("Ikraft").Remove();
            xDoc.Root.Descendants("Nota").Remove();
            xDoc.Root.Descendants("Indledning").Remove();
            MinisterTokenReplacer.ReplaceMinisterReferences(xDoc);
            MinisterTokenReplacer.ReplaceMinisterieReferences(xDoc);
        }
    }
}