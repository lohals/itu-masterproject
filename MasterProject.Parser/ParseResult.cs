namespace Dk.Itu.Rlh.MasterProject.Parser
{
    public class ParseResult<T>
    {
        public T Result { get; set; }
        public IParserErrorResult ErrorResult { get; set; }
    }
}