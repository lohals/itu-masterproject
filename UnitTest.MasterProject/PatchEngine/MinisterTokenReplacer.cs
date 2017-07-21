using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine
{
    public class MinisterTokenReplacer
    {
        private static string _forholdsOrd =
            "om|inden|skal|bør|kan|nedsætter|fastsætter|tilbagekalde|eller|sine|fastsætte|tilkendegive";
        private static Regex _ministerReference = new Regex($@"([M|m]inisteren for.*?(?=(,? ({_forholdsOrd}))|\.)|((\w|[æøåÆØÅ])*- og )+(\w|[æøåÆØÅ])+ministeren)", RegexOptions.Compiled);
        private static Regex _ministerieReference = new Regex($@"((\w|[æøåÆØÅ])*- og )+(\w|[æøåÆØÅ])+ministeriet|[M|m]inisteriet for.*?(?=(,? ({_forholdsOrd}))|\.)", RegexOptions.Compiled);

        public static Match FindMinisterMatches(string input)
        {
            return _ministerReference.Match(input);
        }
        public static void ReplaceMinisterReferences(XDocument xDoc)
        {
            Replace(xDoc, "[MinisterReference]", _ministerReference);
        }
        public static void ReplaceMinisterieReferences(XDocument xDoc)
        {
            Replace(xDoc, "[MinisterieReference]", _ministerieReference);
        }
        private static void Replace(XDocument xDoc, string token, Regex regex)
        {
            foreach (var xText in xDoc.Root.DescendantNodes().OfType<XText>())
            {
                xText.Value = regex.Replace(xText.Value, token);
            }
        }

        public static Match FindMinisterieMatches(string source)
        {
            return _ministerieReference.Match(source);
        }
    }
}