using System.Linq;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition.ParserVisitors
{
    public class SubElementTargetVisitor:AendringDefinitionGrammarBaseVisitor<SubElementTarget>
    {
        public static SubElementTargetVisitor NewInstance { get {return new SubElementTargetVisitor();} }
        public override SubElementTarget VisitQuotedTextChangeExp(AendringDefinitionGrammarParser.QuotedTextChangeExpContext context)
        {
            if (context.QUOTEDTEXT().Count() > 0)
            {
                if (context.QUOTEDTEXT().Count() != 2)
                    return base.VisitQuotedTextChangeExp(context);
                return new SubElementTarget() { Target = context.QUOTEDTEXT(0).GetText(), Replacement = context.QUOTEDTEXT(1).GetText() };
            }
            return base.VisitQuotedTextChangeExp(context);
        }
    }
}
