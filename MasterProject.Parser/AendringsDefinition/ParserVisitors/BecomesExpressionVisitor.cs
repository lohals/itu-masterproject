using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition.ParserVisitors
{
    public class BecomesExpressionVisitor:AendringDefinitionGrammarBaseVisitor<Element>
    {
        public override Element VisitBecomesExp(AendringDefinitionGrammarParser.BecomesExpContext context)
        {
            return context.elementExp()?.Accept(ElementVisitor.NewInstance);
        }
    }
}
