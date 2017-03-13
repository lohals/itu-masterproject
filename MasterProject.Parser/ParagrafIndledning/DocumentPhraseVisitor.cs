using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

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

            //var ti = context.title().Accept(new MyClass());
            //var t = context.title().Accept(new TitleVisitor());
            var t = context.TITLE().GetText().Trim(new[] { 'I',' ' }).TrimEnd(new[] { ',', ' ' });
            //var title = context.title().GetText();
            var reference = context.Accept(new DokumentReferenceVisitor());
            
            return new Dokument { Title=t,DokumentReference=reference };
        }

        public override Dokument VisitDokumentPhraseType2(Grammar.ParagrafIndledningParser.DokumentPhraseType2Context context)
        {
            return base.VisitDokumentPhraseType2(context);
        }
    }
}
