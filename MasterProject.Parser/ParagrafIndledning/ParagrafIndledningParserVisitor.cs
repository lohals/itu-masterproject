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
            var dokumentPhraseType2Context = context.dokumentPhraseType2();
            var phrase2 = dokumentPhraseType2Context?.Accept(new DocumentPhraseVisitor());

            var phrase1 = context.dokumentPhraseType1()?.Accept(new DocumentPhraseVisitor());
           
            return new ParagrafIndledningModel
            {
                Reference = phrase1 ?? phrase2
            };
        }
       
    }
}