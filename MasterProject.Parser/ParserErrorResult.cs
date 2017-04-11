using System.Collections.Generic;
using System.Linq;

namespace Dk.Itu.Rlh.MasterProject.Parser
{
    internal class ParserErrorResult : IParserErrorResult
    {
        private IEnumerable<string> _errors;
        public IEnumerable<string> Verbose { get; }

        public bool HasErrors => _errors.Any();

        public ParserErrorResult(IEnumerable<string> errors, IEnumerable<string> verbose)
        {
            _errors = errors;
            Verbose = verbose;
        }

        public IEnumerable<string> Errors
        {
            get { return _errors; }
        }

        public override string ToString()
        {
            return !HasErrors ? "No errors" : string.Join(";", _errors);
        }
    }
}