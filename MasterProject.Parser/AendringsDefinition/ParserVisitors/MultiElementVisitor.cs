using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition.ParserVisitors
{
    public class MultiElementVisitor : AendringDefinitionGrammarBaseVisitor<Element[]>
    {

        public override Element[] VisitMultiElementExp(AendringDefinitionGrammarParser.MultiElementExpContext context)
        {
            var elements = context.elementChainExp()?
                .Select(c => c.Accept(new ElementChainVisitor()))
                .Where(res=>res!=null).ToArray();
            if (elements?.Any()??false)
                return elements;

            var element = context.elementExp()?.Accept(new ElementVisitor());
            if (element == null)
                throw new NullReferenceException(nameof(element));

            var extra = element.Clone(int.Parse(context.INT()?.GetText()));



            return new []{element,extra}.Where(e => e!=null).ToArray();
        }
        public T ConvertExamp1<T>(T element,ITerminalNode input) where T : Element,new()
        {
            return new T();
        }
        public T ConvertExamp1<T>(object input)
        {
            return (T)Convert.ChangeType(input, typeof(T));
        }
    }
}
