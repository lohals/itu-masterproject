using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition.ParserVisitors
{
    public class MultiQuotedTextChange: AendringDefinitionGrammarBaseVisitor<SubElementTarget[]>
    {
        public override SubElementTarget[] VisitMultiQuotedTextChangeExp(AendringDefinitionGrammarParser.MultiQuotedTextChangeExpContext context)
        {
            return context.quotedTextChangeExp()?
                .Select(expContext => expContext?.Accept(new SubElementTargetVisitor()))
                .Where(target => target != null)
                .ToArray();
        }
    }
}
