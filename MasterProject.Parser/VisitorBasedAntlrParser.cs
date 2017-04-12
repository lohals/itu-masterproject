
using Antlr4.Runtime;
using Dk.Itu.Rlh.MasterProject.Model;
using Dk.Itu.Rlh.MasterProject.Grammar;
using System;
using Antlr4.Runtime.Tree;

namespace Dk.Itu.Rlh.MasterProject.Parser
{

    public abstract class VisitorBasedAntlrParser<T>
    {
        private WhitespaceNormalizer _preBunner;

        public VisitorBasedAntlrParser()
        {
            _preBunner = new WhitespaceNormalizer();
        }
        public ParseResult<T> Parse(string inputString)
        {
            var cleanedString = _preBunner.Clean(inputString);
            var input = new AntlrInputStream(cleanedString);
            var lexer = GetLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var errorListener = new ParserErrorListener();
            var tree = GetParseTree(tokens, errorListener);
            var visitor = GetVisitor();
            return new ParseResult<T>()
            {
                Result = visitor.Visit(tree),
                ErrorResult = errorListener.ErrorResult
            };
        }

        protected abstract IParseTree GetParseTree(CommonTokenStream tokens, BaseErrorListener errorListener);

        protected abstract AbstractParseTreeVisitor<T> GetVisitor();

        protected abstract ITokenSource GetLexer(ICharStream input);       
        

    }

    public class WhitespaceNormalizer
    {
        public string Clean(string inputString)
        {
            return inputString.Replace("\u00A0", " ");//replace non-breaking space with space
        }
    }
}
