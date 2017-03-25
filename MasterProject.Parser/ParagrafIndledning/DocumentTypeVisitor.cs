using System;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;

namespace Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning
{
    public class DocumentTypeVisitor : ParagrafIndledningBaseVisitor<DokumentType>
    {
        public override DokumentType VisitDoctype(Grammar.ParagrafIndledningParser.DoctypeContext context)
        {
            var stringVal = (context.LAW() ?? context.LAWBEKENDTGORELSE()).GetText();
            DokumentType type;
            if(!Enum.TryParse(stringVal, true, out type))
                type=DokumentType.Unknown;
            return type;
        }
    }
}