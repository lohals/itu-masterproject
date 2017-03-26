using Dk.Itu.Rlh.MasterProject.Model;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;
using Xunit;

namespace UnitTest.Dk.Itu.Rlh.MasterProject
{
    public class TestAendringDefinitionModel
    {

        [Fact]
        public void TestAendringDefinitionModelSerializer()
        {
            var sut = new ModelSerializer();
            var input = new AendringDefinition()
            {
                Targets = new[] {new Stk() { NummerStrong = 3,ParentContext = new Paragraf() {NummerStrong = "3 a"} }}
            };
            var serialize = sut.Serialize(input);
            Assert.NotNull(XDocument.Parse(serialize));
        }
    }
}
