using System.Collections.Generic;
using Antlr4.Runtime;

namespace Dk.Itu.Rlh.MasterProject.Parser
{
    public interface IParserErrorResult
    {
        IEnumerable<string> Errors { get; }
        IEnumerable<string> Verbose { get; }
        bool HasErrors { get; } 
    }
}