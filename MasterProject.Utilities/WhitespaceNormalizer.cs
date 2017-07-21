using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MasterProject.Utilities
{
    public static class WhitespaceNormalizer
    {
        public static string NormalizeWhiteSpace(this string inputString)
        {
            return inputString.Replace("\u00A0", " ");//replace non-breaking space with space
        }

        public static IEnumerable<XElement> NormalizeWhiteSpace(this IEnumerable<XElement> elementToInsert)
        {
            foreach (var xElement in elementToInsert)
            foreach (var element in xElement.DescendantsAndSelf())
            foreach (var xText in element.DescendantNodes().OfType<XText>())
            {
                xText.Value = xText.Value.NormalizeWhiteSpace();
            }
            return elementToInsert;
        }

        public static XDocument NormalizeWhiteSpace(this XDocument value)
        {
            new[] { value.Root}.NormalizeWhiteSpace();
            return value;
        }
    }
}