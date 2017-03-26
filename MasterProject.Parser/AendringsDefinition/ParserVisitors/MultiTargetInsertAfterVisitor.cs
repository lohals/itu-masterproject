using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition.ParserVisitors
{
    public class MuultiElementVisitor : AendringDefinitionGrammarBaseVisitor<Element[]>
    {

        public override Element[] VisitMultiElementExp(AendringDefinitionGrammarParser.MultiElementExpContext context)
        {
            var elements = context.elementChainExp()?
                .Select(c => c.Accept(new ElementChainVisitor()))
                .Where(res=>res!=null).ToArray();


            return elements;
        }
    }
}
