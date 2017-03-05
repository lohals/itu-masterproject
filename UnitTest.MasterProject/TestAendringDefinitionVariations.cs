﻿using Dk.Itu.Rlh.MasterProject.Model;
using Dk.Itu.Rlh.MasterProject.Parser;
using System;
using System.Linq;
using Xunit;

namespace UnitTest.Dk.Itu.Rlh.MasterProject
{

    public class TestAendringDefinitionVariations
    {
        [Theory]
        [InlineData("Efter Stk. 3 indsættes som nye stykker:", typeof(Stk), 3, AktionType.IndsaetEfter)]
        //[InlineData("Efter Stk. 3 x:", typeof(Stk),3,AktionType.IndsaetEfter)]
        //[InlineData("Efter Stk. 3 indsættes som:", typeof(Stk),3,AktionType.IndsaetEfter)]
        [InlineData("Efter nr. 3 indsættes som nyt nummer:", typeof(AendringsNummer), 3, AktionType.IndsaetEfter)]
        [InlineData("Efter nr. 42 indsættes som nye numre: ", typeof(AendringsNummer), 42, AktionType.IndsaetEfter)]
        [InlineData("Efter § 46 a indsættes som ny paragraf:", typeof(Paragraf), "46 a", AktionType.IndsaetEfter)]
        [InlineData("Efter § 5 indsættes som ny paragraf:", typeof(Paragraf), "5", AktionType.IndsaetEfter)]
        [InlineData("§ 5, affattes således:", typeof(Paragraf), "5", AktionType.Erstat)]
        [InlineData("§ 5 affattes således:", typeof(Paragraf), "5", AktionType.Erstat)]
        [InlineData("I Stk. 35 ændres »abcæ, 987« til: »xyz: 1.23«.", typeof(Stk), 35, AktionType.Erstat)]
        [InlineData("Nr. 7 udgår.", typeof(AendringsNummer), 7, AktionType.Ophaev)]
        [InlineData("I stk. 7, 2. pkt, ændres »abc 987« til: »xyz 123«.", typeof(Saetning), 2, AktionType.Erstat)]
        [InlineData("I stk. 7 udgår »xyz 123, 987«.", typeof(Stk), 7, AktionType.Ophaev)]
        [InlineData("I nr. 8 ændres »xyz 123, 987« til: »xyz 123«.", typeof(AendringsNummer), 8, AktionType.Erstat)]
        public void WhenValidString_ParserReturnsObject(string inputString
            ,Type expectedTargetType
            ,object expectednumber
            ,AktionType expectedAktionType)
        {

            var parser = new AendringDefintionParser();
            var result = parser.Parse(inputString);

            Assert.Equal((new string[] { }).ToList(), result.ErrorResult.Errors);


            Assert.Equal(expectedAktionType, result.Result.AktionType);

            Assert.Equal(expectednumber, result.Result.Target.Nummer);
            Assert.IsType(expectedTargetType, result.Result.Target);
            
        }

        
        [Theory]
        [InlineData("I stk. 7 udgår »xyz 123, 987«.", "»xyz 123, 987«")]
        public void WhenContainsSingleQuotedTextAndAutomationIsPossible_QuotedTextAreSetInSubElementTarget(string inputString, string expectedTarget)
        {
            //ændres »abc 987« til: »xyz 123«.
            var parser = new AendringDefintionParser();
            var result = parser.Parse(inputString);

            Assert.Equal((new string[] { }).ToList(), result.ErrorResult.Errors);

            Assert.Equal(expectedTarget, result.Result.Target.SubElementTarget.Target);
            Assert.Null(result.Result.Target.SubElementTarget.Replacement);

        }

        [Theory]
        [InlineData("I stk. 9 ændres »abc, 93; øæå« til: »xyz 123. df,ø ,å, åå«.", "»abc, 93; øæå«", "»xyz 123. df,ø ,å, åå«")]
        [InlineData("I det under nr. 9 foreslåede § 6, stk. 10, ændres »abc 987:;, øæå <>#¤%&« til: »xyz 123«.", "»abc 987:;, øæå <>#¤%&«", "»xyz 123«")]
        [InlineData("I nr. 8 ændres »xyz 123, 987« til: »xyz 123«.", "»xyz 123, 987«", "»xyz 123«")]
        public void WhenContainsQuotedTextChanged_QuotedTextArePreserved(string inputString, string expectedTarget, string expectedReplacement)
        {
            //ændres »abc 987« til: »xyz 123«.
            var parser = new AendringDefintionParser();
            var result = parser.Parse(inputString);

            Assert.Equal(new string[] { }.ToList(), result.ErrorResult.Errors);

            Assert.Equal(expectedTarget, result.Result.Target.SubElementTarget.Target);
            Assert.Equal(expectedReplacement, result.Result.Target.SubElementTarget.Replacement);

        }

        public void WhenSpecialCaseWithMulitpleElements_ElementStructureIsCorrect(string inputString, string expectedTarget, string expectedReplacement)
        {
            //ændres »abc 987« til: »xyz 123«.
            var parser = new AendringDefintionParser();
            var result = parser.Parse(inputString);

            Assert.Equal((new string[] { }).ToList(), result.ErrorResult.Errors);

            Assert.Equal(expectedTarget, result.Result.Target.SubElementTarget.Target);
            Assert.Equal(expectedReplacement, result.Result.Target.SubElementTarget.Replacement);

        }
        [Theory]
        [InlineData("random input","asdfhello 2 world")]
        [InlineData("Nummer not formatted correctly","Før 20 indsættes som nyt nummer:")]
        [InlineData("extra input supplied after valid expression","Efter § 100 indsættes: støj")]
        [InlineData("Illegal double phrase ending","Efter § 100 indsættes::")]
        [InlineData("Illegal double different phrase ending","Efter § 100 indsættes:.")]
        public void WhenInvalidString_ErrorsAreReported(string description, string inputString)
        {

            var parser = new AendringDefintionParser();
            var result = parser.Parse(inputString);
            Assert.NotEqual(Enumerable.Empty<string>(), result.ErrorResult.Errors);
        }

    }
}
