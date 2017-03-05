using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.ParagrafIndledningParser
{
    class TestParagrafIndledning
    {
        [Theory]
        [InlineData("I økologilov, lov nr. 463 af 17. juni 2008, foretages følgende ændring:")]
        public void TestSingleDocumentReference(string input)
        {

        }
    }
}
