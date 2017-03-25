using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;
using Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition.ParserVisitors;

namespace Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition
{
    public class AendringDefintionParser : VisitorBasedAntlrParser<AendringDefinition>
    {

        protected override ITokenSource GetLexer(ICharStream input)
        {
            return new AendringDefinitionGrammarLexer(input);
        }

        protected override IParseTree GetParseTree(CommonTokenStream tokens, BaseErrorListener errorListener)
        {
            var parser = new AendringDefinitionGrammarParser(tokens);
            parser.AddErrorListener(errorListener);

            return parser.aendringDefinition();
            
        }

        protected override AbstractParseTreeVisitor<AendringDefinition> GetVisitor()
        {
            return new AendringDefinitionVisitor();
        }
    }
}
