using System;
using System.Collections.Generic;
using System.Linq;
using Dk.Itu.Rlh.MasterProject.Model;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;
using Dk.Itu.Rlh.MasterProject.Parser;
using Dk.Itu.Rlh.MasterProject.Parser.AendringsDefinition;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.AendringsDefinitionParser
{
    public class TestConcreteDefinitionsForCorrectParsing
    {
        private static string _quotedTextTemplate = "»{0}«";
        private static string _citerettekst = "[CiteretTekst]";
        private static int _numberOfTestCases = 10;
        private readonly AendringDefintionParser _sut;

        public TestConcreteDefinitionsForCorrectParsing()
        {
            _sut = new AendringDefintionParser();
        }

        
        public static IEnumerable<object[]> SimpleParentContextTargetTestData
        {
            get
            {
                return
                    new []
                    {
                        new object[] {"Paragraffen affattes således:", AktionType.Erstat, typeof(ParentElementContext)},
                        new object[] { "Paragraffen udgår. ",AktionType.Ophaev,typeof(ParentElementContext) }
                    }
                   ;
            }
        }

        [Theory]
        [InlineData("§ 15, stk. 2, 1. pkt, affattes således:", new object[] { 1,2, "15" }, new[] { typeof(Saetning), typeof(Stk), typeof(Paragraf) })]
        [InlineData("§ 15, stk. 2, affattes således:", new object[] { 2, "15" },  new[] { typeof(Stk), typeof(Paragraf) })]
        [InlineData("§ 153, stk. 1, ophæves, og i stedet indsættes:", new object[] { 1, "153" }, new Type[] { typeof(Stk), typeof(Paragraf) })]
        [InlineData("§ 198 b ophæves, og i stedet indsættes:", new object[] { "198 b" },  new []{ typeof(Paragraf) })]
        public void Test_ErstatExpressions(string input, object[] expectedExplicatus,  Type[] expectedTypes)
        {
            TestParseResult(input, expectedExplicatus, AktionType.Erstat, expectedTypes);
        }
        [Theory]
        [InlineData("§ 15, stk. 2, ophæves.", new object[] { 2, "15" }, new[] { typeof(Stk), typeof(Paragraf) })]
        [InlineData("§ 15, stk. 2, 1. pkt, ophæves.", new object[] { 1 , 2, "15" },  new[] { typeof(Saetning), typeof(Stk), typeof(Paragraf) })]
        public void Test_OphaevExpressions(string input, object[] expectedExplicatus, Type[] expectedTypes)
        {
            TestParseResult(input, expectedExplicatus, AktionType.Ophaev, expectedTypes);
        }
        [Theory]
        [InlineData("I § 15 indsættes efter stk. 2 som nyt stykke:", new object[] { 2, "15" }, new[] { typeof(Stk), typeof(Paragraf) })]
        [InlineData("Efter § 100 indsættes:", new object[] {100 }, new Type[] { typeof(Paragraf), })]
        [InlineData("I § 5 a indsættes efter stk. 2 som nyt stykke:", new object[] { 2, "5 a" },  new Type[] { typeof(Stk), typeof(Paragraf) })]
        [InlineData("I § 58 indsættes efter nr. 1 som nyt nummer:", new object[] { 1, 58 },  new Type[] { typeof(NummerOpregningElement), typeof(Paragraf) })]
        [InlineData("I § 148 b indsættes som stk. 4 og 5:", new object[] { 3, "148 b" },  new Type[] { typeof(Stk), typeof(Paragraf) })]
        [InlineData("I § 3 a indsættes som stk. 3:", new object[] { 2, "3 a" },  new[] { typeof(Stk), typeof(Paragraf) })]
        [InlineData("I § 15, stk. 2, indsættes som 4. pkt:", new object[] { 3, 2, "15" }, new[] { typeof(Saetning), typeof(Stk), typeof(Paragraf) })]
        [InlineData("I § 8 indsættes efter stk. 3 som nyt stykke:",new object[] { 3,8}, new Type[] { typeof(Stk), typeof(Paragraf) })]
        public void Test_IndsaetEfterExpressions(string input, object[] expectedExplicatus, Type[] expectedTypes)
        {
            TestParseResult(input, expectedExplicatus, AktionType.IndsaetEfter, expectedTypes);
        }

        [Theory]
        [InlineData("I § 148 b indsættes før stk. 1 som nye stykker:", new object[] { 1, "148 b" },  new [] { typeof(Stk), typeof(Paragraf) })]
        [InlineData("I § 154 a, stk. 1, indsættes før nr. 1 som nye numre:", new object[] {1, 1, "154 a" }, new Type[] { typeof(NummerOpregningElement), typeof(Stk), typeof(Paragraf) })]
        public void Test_IndsaetFoerExpressions(string input, object[] expectedExplicatus, Type[] expectedTypes)
        {
            TestParseResult(input,expectedExplicatus,AktionType.IndsaetFoer, expectedTypes);
        }
        [Theory]
        [InlineData("I § 197, stk. 1, 1. pkt., udgår »modparten og«, og i 2. pkt. udgår »parten og«.", new object[] { 1,1,"197" }, new[] { typeof(Saetning),typeof(Stk), typeof(Paragraf), })]
        [InlineData("§ 348, stk. 3, 2. pkt., ophæves, og i stk. 4, 1. pkt., udgår »kopier af denne og«, og 2. pkt. ophæves.", new object[] { 2,3,"348" }, new[] { typeof(Saetning),typeof(Stk), typeof(Paragraf), })]
        [InlineData("I § 349, stk. 1, 1. pkt., udgår »stk. 3, 2. pkt.,«, og i 2. pkt. indsættes efter »stk. 4«: », eller hvis stævningen eller en anden anmodning til retten ikke indleveres til retten i overensstemmelse med kravene i § 148 a, stk. 1«.", new object[] { 1,1,"349" }, new[] { typeof(Saetning),typeof(Stk), typeof(Paragraf), })]
        [InlineData("§ 38 og § 39, stk. 1, 2. og 3. pkt., ophæves.", new object[] { "38" }, new[] { typeof(Paragraf), })]
        //[InlineData("§ 351, stk. 5, 2. og 3. pkt., ophæves, og stk. 6, 1. og 2. pkt., ophæves, og i stedet indsættes:", new object[] { 5,"351" }, new[] { typeof(Stk), typeof(Paragraf), })]
        public void Test_ManuelExpressions(string input, object[] expectedExplicatus, Type[] expectedTypes)
        {
            TestParseResult(input, expectedExplicatus, AktionType.Manuel, expectedTypes);
        }
        private void TestParseResult(string input, object[] expectedExplicatus, AktionType expectedAktionType, Type[] expectedTypes)
        {
            var parseResult = _sut.Parse(input);
            AssertTargetElementChain(parseResult, expectedExplicatus, expectedAktionType, expectedTypes);
        }

        private void AssertTargetElementChain(ParseResult<AendringDefinition> parseResult, object[] expectedExplicatus, AktionType expectedAktionType,
            Type[] expectedType)
        {
            //Assert.Equal(Enumerable.Empty<string>(), parseResult.ErrorResult.Errors); //no errors
            Assert.Equal(expectedType.Count(), parseResult.Result.Target.GetAncestorsAndSelf.Count()); //one target level
            Assert.Equal(expectedAktionType, parseResult.Result.AktionType);

            //assert chain
            int counter = 0;
            Assert.All(parseResult.Result.Target.GetAncestorsAndSelf, element =>
            {
                Assert.IsType(expectedType[counter], element);
                Assert.Equal(expectedExplicatus[counter].ToString(), element.Nummer.ToString());
                counter++;
            });
        }

        [Theory]
        //Nested citations not supported yet!!  [InlineData("I stk. 3 ændres »123»abcd«xyz« til: »9 8 7 6 5 4 3«.", new object[] {3} , "»123»abcd«xyz«", "»9 8 7 6 5 4 3«", AktionType.Erstat, new Type[] { typeof(Stk)})]
        [InlineData("I stk. 3 ændres »9607ee90-3697-4bae-8969-9091b5321« til: »1234ee90-3697-4bae-8969-9091b5321«.", new object[] {3} , "»9607ee90-3697-4bae-8969-9091b5321«", "»1234ee90-3697-4bae-8969-9091b5321«", AktionType.Erstat, new Type[] { typeof(Stk)})]
        [InlineData("I stk. 10 indsættes efter »9607ee90-3697-4bae-8969-9091b5321«: »1234ee90-3697-4bae-8969-9091b5321«.", new object[] {10}, "»9607ee90-3697-4bae-8969-9091b5321«", "»1234ee90-3697-4bae-8969-9091b5321«", AktionType.IndsaetEfter, new Type[] { typeof(Stk)})]
        [InlineData("I § 5 a, stk. 10, indsættes efter »9607ee90-3697-4bae-8969-9091b5321«: »1234ee90-3697-4bae-8969-9091b5321«.", new object[] {10,"5 a"}, "»9607ee90-3697-4bae-8969-9091b5321«", "»1234ee90-3697-4bae-8969-9091b5321«", AktionType.IndsaetEfter, new Type[] { typeof(Stk),typeof(Paragraf)})]
        [InlineData("I § 10, stk. 7, nr. 4, ændres »9607ee90-3697-4bae-8969-9091b5321« til: »1234ee90-3697-4bae-8969-9091b5321«.", new object[] {4,7,10}, "»9607ee90-3697-4bae-8969-9091b5321«", "»1234ee90-3697-4bae-8969-9091b5321«", AktionType.Erstat, new Type[] {typeof(NummerOpregningElement), typeof(Stk),typeof(Paragraf)})]
        [InlineData("I § 448 b, 1. pkt., ændres »social- og indenrigsministeren eller den, ministeren bemyndiger dertil,« til: »Ankestyrelsen«.", new object[] { 1,"448 b"}, "»social- og indenrigsministeren eller den, ministeren bemyndiger dertil,«", "»Ankestyrelsen«", AktionType.Erstat, new Type[] { typeof(Saetning), typeof(Paragraf) })]
        public void Test_Simple_SubelementTargetLevel_AendringDefinitioner(string input, object[] expectedExplicatus, string quotedFrom,
            string quotedTo,AktionType expectedAktionType,Type[] aendringsType)
        {
            var parseResult = _sut.Parse(input);
            AssertTargetElementChain(parseResult, expectedExplicatus,expectedAktionType,aendringsType);
            Assert.Equal(quotedFrom,parseResult.Result.Target.SubElementTarget.Target);
            Assert.Equal(quotedTo,parseResult.Result.Target.SubElementTarget.Replacement);
        }

        [Theory]
        [InlineData("I stk. 3 udgår »9607ee90-3697-4bae-8969-9091b5321«.", 3, "»9607ee90-3697-4bae-8969-9091b5321«", typeof(Stk))]
        public void Test_RemoveSubElementTarget<T>(string input, T expectedExplicatus, string quotedFrom, Type targetType)
        {
            var parseResult = _sut.Parse(input);
            Assert.Equal(0, parseResult.ErrorResult.Errors.Count()); //no errors
            Assert.Equal(1, parseResult.Result.Target.GetAncestorsAndSelf.Count()); //one target level
            Assert.IsType(targetType, parseResult.Result.Target);
            Assert.Equal(expectedExplicatus, ((Element<T>)parseResult.Result.Target).NummerStrong);
            Assert.Equal(quotedFrom, parseResult.Result.Target.SubElementTarget.Target);
            Assert.Null(parseResult.Result.Target.SubElementTarget.Replacement);
            Assert.Equal(AktionType.Ophaev, parseResult.Result.AktionType);
        }

        [Theory]
        [MemberData(nameof(SimpleParentContextTargetTestData))]
        public void Test_ParentContextTargetIsParsedCorrectly(string input,AktionType aktionType, Type expectedTarget)
        {
            var parseResult = _sut.Parse(input);
            AssertErrors(parseResult);
            Assert.Equal(1, parseResult.Result.Target.GetAncestorsAndSelf.Count()); //one target level
            Assert.IsType(expectedTarget, parseResult.Result.Target);
            Assert.Null(parseResult.Result.Target.Nummer);
            Assert.Equal(aktionType, parseResult.Result.AktionType);
        }

        private static void AssertErrors(ParseResult<AendringDefinition> parseResult)
        {
            Assert.Equal(0, parseResult.ErrorResult.Errors.Count()); //no errors
        }
    }
}
