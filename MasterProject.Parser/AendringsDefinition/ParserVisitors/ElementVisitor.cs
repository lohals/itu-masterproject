using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model;

namespace Dk.Itu.Rlh.MasterProject.Parser.ParserVisitors
{
    public class ElementVisitor: AendringDefinitionGrammarBaseVisitor<Element>
    {

        public static ElementVisitor NewInstance {
            get { return new ElementVisitor(); }
        }

        public override Element VisitPktExp(AendringDefinitionGrammarParser.PktExpContext context)
        {
           return InstanceOf<Saetning>(context.INT());
        }

        private static T InstanceOf<T>(ITerminalNode node) where T:Element<int>
        {
            return new ElementWithIntegerNummerVisitorHelper<T>(node).Instance;
        }

        public override Element VisitStkExp([NotNull] AendringDefinitionGrammarParser.StkExpContext context)
        {
            return InstanceOf<Stk>(context.INT());
        }
        
        public override Element VisitAendringsNummerExp([NotNull] AendringDefinitionGrammarParser.AendringsNummerExpContext context)
        {
            return InstanceOf<AendringsNummer>(context.INT());
        }

        public override Element VisitNummerOpregningExp(AendringDefinitionGrammarParser.NummerOpregningExpContext context)
        {
            return InstanceOf<NummerOpregningElement>(context.INT());
        }

        public override Element VisitParagrafExp(AendringDefinitionGrammarParser.ParagrafExpContext context)
        {
            var nummer = ParseInt(context.INT());
            if (!nummer.HasValue)
                return null;

            var letter = context.LETTER() == null ? string.Empty : $" {context.LETTER().GetText()}";
            return new Paragraf() { NummerStrong = $"{nummer.Value}{letter}" };
        }
        private int? ParseInt(ITerminalNode node)
        {
            if (node == null)
                return null;
            int s;
            if (int.TryParse(node.GetText(), out s))
            {
                return s;
            }
            return null;
        }

    }
}