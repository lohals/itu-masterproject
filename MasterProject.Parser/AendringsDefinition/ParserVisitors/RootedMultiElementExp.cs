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
            if(multiElement==null)
                throw new NullReferenceException(nameof(multiElement));
            foreach (var element in multiElement)
            {
                element.GetAncestorsAndSelf.Reverse().First().ParentContext = commonParent;
            }
            return multiElement;
        }
    }
}
