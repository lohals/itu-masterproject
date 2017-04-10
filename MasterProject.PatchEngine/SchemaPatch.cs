namespace MasterProject.PatchEngine
{
    public class SchemaPatch : IPatchTask
    {
        private readonly string _lbkSchemaLocation;

        public SchemaPatch(string lbkSchemaLocation)
        {
            _lbkSchemaLocation = lbkSchemaLocation;
        }

        public void Patch(TargetDocument targetDocument, ChangeDocument[] changeDocuments)
        {
            targetDocument.Source.Root.Attribute("SchemaLocation").Value = _lbkSchemaLocation;
        }
    }
}