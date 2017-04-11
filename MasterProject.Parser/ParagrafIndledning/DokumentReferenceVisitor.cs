using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;

namespace Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning
{
    public class DokumentReferenceVisitor: ParagrafIndledningBaseVisitor<DokumentReferenceData>
    {
        public override DokumentReferenceData VisitDokumentReference([NotNull] Grammar.ParagrafIndledningParser.DokumentReferenceContext context)
        {
            var number = int.Parse(context.INT()?.GetText()??"-1");
            var year = context.date()?.Accept(new YearVisitor());
            if (!year.HasValue)
                return null;

            var type = context.doctype().Accept(new DocumentTypeVisitor());

            return new DokumentReferenceData
            {
                DokumentType = type,
                Number = number,
                Year=year.Value
            };
        }
    }
}
