using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning
{
    public class DokumentReferenceVisitor: ParagrafIndledningBaseVisitor<DokumentReferenceData>
    {
        public override DokumentReferenceData VisitDokumentReference([NotNull] Grammar.ParagrafIndledningParser.DokumentReferenceContext context)
        {
            var number = int.Parse(context.INT()?.GetText());
            var year = context.date().Accept(new YearVisitor());
            return new DokumentReferenceData
            {
                Number = number,
                Year=year
            };
        }
    }
}
