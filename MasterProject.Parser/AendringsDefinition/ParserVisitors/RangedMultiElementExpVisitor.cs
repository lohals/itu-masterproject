using System;
using System.Collections.Generic;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition.ParserVisitors
{
    public class RangedMultiElementExpVisitor : AendringDefinitionGrammarBaseVisitor<Element[]>
    {
        public override Element[] VisitRangedMultiElementExp(AendringDefinitionGrammarParser.RangedMultiElementExpContext context)
        {
            var baseElement = context.elementExp()?.Accept(new ElementVisitor());
            var endRange = int.Parse(context.INT()?.GetText()??"-1");
            if (endRange == -1 ||!(baseElement?.Nummer is int))
                return new[] {baseElement};

            var startRange = (int) baseElement.Nummer;
            var list = new List<Element>();
            list.Add(baseElement);
            for (int i = (startRange+1); i <= endRange; i++)
            {
                list.Add(baseElement.Clone(i));
            }
            return list.ToArray();
        }
    }
}