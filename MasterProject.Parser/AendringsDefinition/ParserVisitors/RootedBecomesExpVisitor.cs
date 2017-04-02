using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition.ParserVisitors
{
    public class RootedBecomesExpVisitor : AendringDefinitionGrammarBaseVisitor<Element>
    {
        public override Element VisitRootedBecomesExp(AendringDefinitionGrammarParser.RootedBecomesExpContext context)
        {
            var root = context.rootElementExp()?.Accept(new ElementVisitor());
            var becomesChain = context.becomesChainExp()?.Accept(new ElementChainVisitor());
            if (becomesChain == null)
                return root;
            becomesChain.GetAncestorsAndSelf.Reverse().First().ParentContext=root;

            return becomesChain;
        }
        
    }
}
