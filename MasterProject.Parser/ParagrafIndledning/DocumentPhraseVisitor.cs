using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;

namespace Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning
{
    
    //class TitleVisitor:ParagrafIndledningBaseVisitor<string>
    //{

    //    public override string VisitTitle([NotNull] Grammar.ParagrafIndledningParser.TitleContext context)
    //    {
    //        //var c  = context.TITLE().
    //        return context.FREETEXT()?.GetText()??string.Empty;
    //    }

    //}
    public class DocumentPhraseVisitor: ParagrafIndledningBaseVisitor<Dokument>
    {

        public override Dokument VisitDokumentPhraseType1([NotNull] Grammar.ParagrafIndledningParser.DokumentPhraseType1Context context)
        {
            var debug = context.GetText();
            
            var titlenode = context.TITLE();
            var title = titlenode.GetText().Trim(new[] { 'I',' ' }).TrimEnd(new[] { ',', ' ' });

            var dokumentReferenceContext = context.dokumentReference();
            var reference = GetReference(dokumentReferenceContext);

            return BuildDokument(reference, title);
        }

        private static DokumentReferenceData GetReference(Grammar.ParagrafIndledningParser.DokumentReferenceContext dokumentReferenceContext)
        {
            return dokumentReferenceContext?.Accept(new DokumentReferenceVisitor());
        }

        public override Dokument VisitDokumentPhraseType2(Grammar.ParagrafIndledningParser.DokumentPhraseType2Context context)
        {
            var debug = context.GetText();

            var rawSplitTitlePart = context.SPLIT_TITLE().GetText();
            var reference = GetReference(context.dokumentReference());

            var findFirstValidIndex = FindFirstSplitTitleEndingIndex( rawSplitTitlePart);

            var title = findFirstValidIndex > -1
                ? $"{reference.DokumentType} {rawSplitTitlePart.Substring(0, findFirstValidIndex)}"
                : $"{reference.DokumentType} {rawSplitTitlePart}";

            return BuildDokument(reference, title.Trim());
        }

        private static int FindFirstSplitTitleEndingIndex(string rawSplitTitlePart)
        {
            var splitTitleEnding = new[] { "foretages følgende ændring", "som ændret ved" };

            var findFirstValidIndex = splitTitleEnding
                .Select(s => rawSplitTitlePart.IndexOf(s, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault(i => i > -1);

            return findFirstValidIndex;
        }

        private static Dokument BuildDokument(DokumentReferenceData reference, string title)
        {
            return new Dokument {Title = title, DokumentReference = reference};
        }
    }
}
