using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition.ParserVisitors
{
    public class ElementChainVisitor:AendringDefinitionGrammarBaseVisitor<Element>
    {

        public override Element VisitElementChainExp(AendringDefinitionGrammarParser.ElementChainExpContext context)
        {
            var aendringsNummerExpression= context.aendringsNummerExp();
            Element nummer=null;
            if(aendringsNummerExpression != null)
                nummer = aendringsNummerExpression.Accept(ElementVisitor.NewInstance);
            var elements = MapElementExpressions(context.elementExp());

            var opregningsElement = MapElementExpressions(context.opregningExp());
            if (opregningsElement.Any())
                opregningsElement.FirstOrDefault().ParentContext = elements.LastOrDefault();
            BindTogetherAendringsNummerWithElementChain(elements, nummer);

            //returns the inner most element in the nested chain (which is the last in the list or aendrigsnummer if exists and chain is empty)
            return GetInnerMostTargetElement(nummer, elements.LastOrDefault(),opregningsElement.LastOrDefault());
        }

        private Element GetInnerMostTargetElement(params Element[] element)
        {
            return element.Where(e => e != null).LastOrDefault();
        }

        private void BindTogetherAendringsNummerWithElementChain(IList<Element> elementChain, Element aendringsNummer)
        {
            //If aendrings nummer was present it is assumed it is always the first (as also described in the grammar)
            //therefore we set it to the outermost parent (the first in the chain list).
            if (elementChain.Any() && aendringsNummer != null)
                elementChain.First().ParentContext = aendringsNummer;
        }

        public IList<Element> MapElementExpressions(IReadOnlyList<ParserRuleContext> elementContext)
        {
            var targets = elementContext
                .Select(exp => exp.Accept(ElementVisitor.NewInstance))
                .Where(exp => exp != null).ToList();

            Element currentParent = null;

            foreach (var item in targets)
            {
                if (currentParent != null)
                    item.ParentContext = currentParent;
                currentParent = item;
            }
            return targets;
        }
    }
}
