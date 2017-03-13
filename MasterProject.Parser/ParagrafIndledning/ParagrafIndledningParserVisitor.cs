using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;
using System.Collections.Generic;

namespace Dk.Itu.Rlh.MasterProject.Parser.ParagrafIndledning
{
    internal class ParagrafIndledningParserVisitor : ParagrafIndledningBaseVisitor<ParagrafIndledningModel>
    {
        public override ParagrafIndledningModel VisitParagrafIndledning([NotNull] Grammar.ParagrafIndledningParser.ParagrafIndledningContext context)
        {
            var dokumentPhraseContext = context.dokumentPhraseType1();
            var dokument = dokumentPhraseContext.Accept(new DocumentPhraseVisitor());
             return new ParagrafIndledningModel
            {
                References = new List<Dokument>()
                {
                    dokument
                }
            };
        }
       
    }
}