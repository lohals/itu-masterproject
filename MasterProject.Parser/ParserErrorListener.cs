using System.Collections.Generic;
using Antlr4.Runtime;
namespace Dk.Itu.Rlh.MasterProject.Parser
{
    public class ParserErrorListener:BaseErrorListener
    {
        private readonly IList<string> _errors = new List<string>();
        private IList<string> _verbose = new List<string>();

        public IParserErrorResult ErrorResult
        {
            get { return new ParserErrorResult(_errors,_verbose); }
        }

        
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
           _errors.Add(msg);
            _verbose.Add(string.Join("\n",new [] {
                $"recognizer: {recognizer}",
                $"offendingSymbol: {recognizer}",
                $"line: {line}",
                $"charPositionInLine: {charPositionInLine}",
                $"msg: {msg}",
                //$"RecognitionException: {e}",
            })
                );
            base.SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e);
        }
    }
}
