using Dk.Itu.Rlh.MasterProject.Grammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning
{
    public class YearVisitor : ParagrafIndledningBaseVisitor<int>
    {
        public override int VisitYear([NotNull] Grammar.ParagrafIndledningParser.YearContext context)
        {
            return int.Parse(context.INT().GetText() ?? "-1");
        }
        
    }
}
