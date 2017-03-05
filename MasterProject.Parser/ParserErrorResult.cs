using System.Collections.Generic;

namespace Dk.Itu.Rlh.MasterProject.Parser
{
    internal class ParserErrorResult : IParserErrorResult
    {
        private IEnumerable<string> _errors;
        public IEnumerable<string> Verbose { get; }

        public ParserErrorResult(IEnumerable<string> errors, IEnumerable<string> verbose)
        {
            _errors = errors;
            Verbose = verbose;
        }

        public IEnumerable<string> Errors
        {
            get { return _errors; }
        }
    }
}