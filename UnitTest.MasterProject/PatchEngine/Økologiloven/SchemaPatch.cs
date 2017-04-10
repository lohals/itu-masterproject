using System.Xml.Linq;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine.Økologiloven
{
    public class SchemaPatch : IPatchTask
    {
        private readonly string _lbkSchemaLocation;

        public SchemaPatch(string lbkSchemaLocation)
        {
            _lbkSchemaLocation = lbkSchemaLocation;
        }

        public void Patch(XDocument source, ChangeDocument[] changeDocuments)
        {
            source.Root.Attribute("SchemaLocation").Value = _lbkSchemaLocation;
        }
    }
}