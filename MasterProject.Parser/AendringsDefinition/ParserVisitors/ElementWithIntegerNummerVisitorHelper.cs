using System;
using Antlr4.Runtime.Tree;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition.ParserVisitors
{
    public class ElementWithIntegerNummerVisitorHelper<T> where T:Element<int>
    {
        private readonly ITerminalNode _node;

        public ElementWithIntegerNummerVisitorHelper(ITerminalNode node)
        {
            _node = node;

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
        public T Instance
        {
            get
            {
                var value = ParseInt(_node);
                if (!value.HasValue)
                    return default(T);
                var c = Activator.CreateInstance<T>();
                c.NummerStrong = value.Value;
                return c;
            }
        }
    }
}