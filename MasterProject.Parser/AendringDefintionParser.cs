
using Antlr4.Runtime;
using Dk.Itu.Rlh.MasterProject.Model;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Parser.ParserVisitors;

namespace Dk.Itu.Rlh.MasterProject.Parser
{
    public class AendringDefintionParser
    {
        public ParseResult<AendringDefinition> Parse(string inputString)
        {
            AntlrInputStream input = new AntlrInputStream(inputString);
            AendringDefinitionGrammarLexer lexer = new AendringDefinitionGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            AendringDefinitionGrammarParser parser = new AendringDefinitionGrammarParser(tokens);

            var errorListener = new ParserParserErrorListener();
            parser.AddErrorListener(errorListener);
            var tree = parser.aendringDefinition();
            var syntaxTree = tree.ToStringTree(parser);
            var visitor = new AendringDefinitionVisitor();
            return new ParseResult<AendringDefinition>()
            {
                Result = visitor.Visit(tree) as AendringDefinition,
                ErrorResult = errorListener.ErrorResult
            };
        }
    }
}
