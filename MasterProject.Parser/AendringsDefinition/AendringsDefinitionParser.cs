using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model;
using Dk.Itu.Rlh.MasterProject.Parser.ParserVisitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dk.Itu.Rlh.MasterProject.Parser
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
