using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition.ParserVisitors
{
    public class RootedMultiElementExp : AendringDefinitionGrammarBaseVisitor<Element[]>
    {
        public override Element[] VisitRootedMultiElementExp(AendringDefinitionGrammarParser.RootedMultiElementExpContext context)
        {
            var commonParent = context.rootElementExp()?.Accept(new ElementVisitor());
            var multiElement = context.multiElementExp()?.Accept(new MultiElementVisitor());
            var rangedElements = context.rangedMultiElementExp()?.Accept(new RangedMultiElementExpVisitor());

            var targetArray = multiElement?? rangedElements??Enumerable.Empty<Element>().ToArray();
            foreach (var element in targetArray)
            {
                element.GetAncestorsAndSelf.Reverse().First().ParentContext = commonParent;
            }
            return targetArray;
        }
    }
}
