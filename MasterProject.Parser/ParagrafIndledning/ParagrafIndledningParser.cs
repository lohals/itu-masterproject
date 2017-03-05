using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;
using Dk.Itu.Rlh.MasterProject.Grammar;

namespace Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning
{
    public class ParagrafIndledningParser : VisitorBasedAntlrParser<ParagrafIndledningModel>
    {
        protected override ITokenSource GetLexer(ICharStream input)
        {
            return new ParagrafIndledningLexer(input);
        }

        protected override IParseTree GetParseTree(CommonTokenStream tokens, BaseErrorListener errorListener)
        {
            var parser = new Dk.Itu.Rlh.MasterProject.Grammar.ParagrafIndledningParser(tokens);
            parser.AddErrorListener(errorListener);
            var result = parser.paragrafIndledning();
            return result;
        }

        protected override AbstractParseTreeVisitor<ParagrafIndledningModel> GetVisitor()
        {
            return new ParagrafIndledningParserVisitor();
        }
    }
}
