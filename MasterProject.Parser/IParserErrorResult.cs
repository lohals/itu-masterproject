using System.Collections.Generic;

namespace Dk.Itu.Rlh.MasterProject.Parser
{
    public interface IParserErrorResult
    {
        IEnumerable<string> Errors { get; }
        IEnumerable<string> Verbose { get; }
    }
}