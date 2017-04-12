using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        [InlineData("I § 86, stk. 3, ændres »justitsministeren og social- og indenrigsministeren« til: »justitsministeren, børne- og socialministeren og udlændinge- og integrationsministeren«, og »§ 82 a« ændres til: »§§ 82 a og 87«."
            , new object[]
            {
                new object[] { "justitsministeren og social- og indenrigsministeren", "justitsministeren, børne- og socialministeren og udlændinge- og integrationsministeren" },
                new object[] { "§ 82 a", "§§ 82 a og 87" },
            }
            ,new object[] {3,"86"}
            ,new[] {typeof(Stk),typeof(Paragraf)})]
        [InlineData("I § 86, stk. 3, nr. 3, ændres »på sikret afdeling« til: »på en sikret døgninstitution«, og »lov om social service, eller« ændres til »lov om social service, herunder som led i en strafferetlig afgørelse afsagt ved dom eller kendelse, eller«."
            ,new object[]
            {
                new object[] { "på sikret afdeling", "på en sikret døgninstitution" },
                new object[] { "lov om social service, eller", "lov om social service, herunder som led i en strafferetlig afgørelse afsagt ved dom eller kendelse, eller" },
            }
            , new object[] { 3, 3, "86" }
            , new[] {  typeof(NummerOpregningElement),typeof(Stk)
                , typeof(Paragraf) })]
        public void TestSubElementMultiTarget_QuotedTextChanged(string input, object[][] expectedSubElementTargets,object[]explicatusChain,Type[] typeChain)
        {
            var result = _sut.Parse(input);
            AssertErrors(result);
            Assert.Equal(1,result.Result.Targets.Length);
            AssertTargetElementChain(explicatusChain,AktionType.Erstat, typeChain,result.Result,result.Result.Target);
            AssertSubElementMultiReplaceChange(expectedSubElementTargets,result.Result.Target.SubElementTargets);
        }
        [Theory]
        [InlineData("Overalt i loven ændres »Styrelsen for Arbejdsmarked og Rekruttering« til: »Styrelsen for International Rekruttering og Integration«."
            ,new object[] {new object[] { "Styrelsen for Arbejdsmarked og Rekruttering", "Styrelsen for International Rekruttering og Integration" } })]
        public void TestGlobalTarget_QuotedTextReplace(string input,object[][] subElementTargets)
        {
            var result = _sut.Parse(input);
            AssertErrors(result);
            Assert.IsType<DokumentElement>(result.Result.Target);
            Assert.Equal(1,result.Result.Targets.Length);
            Assert.Null(result.Result.Target.ParentContext);
            AssertSubElementMultiReplaceChange(subElementTargets, result.Result.Target.SubElementTargets);
        }

        private static void AssertSubElementMultiReplaceChange(object[][] expectedSubElementTargets, SubElementTarget[] subElementTargets)
        {
            Assert.Equal(subElementTargets.Length, subElementTargets.Length);
            int counter = 0;
            Assert.All(subElementTargets, st =>
            {
                Assert.Equal<object>(expectedSubElementTargets[counter][0], st.Target);
                Assert.Equal<object>(expectedSubElementTargets[counter][1], st.Replacement);
                counter++;
            });
        }

        [Theory]
        [InlineData("I § 25, stk. 4, og § 26, stk. 5, ændres »hjælp« til: »personlig bistand«."
            , new object[] {new object[] {4,25}, new object[] { 5,26 } }
            ,new object[]{new[] { typeof(Stk),typeof(Paragraf)}, new[] { typeof(Stk), typeof(Paragraf) }}
            , "hjælp"
            , "personlig bistand")]
        public void TestMultiTarget_QuotedTextReplace(string input,object[][] explicatusChains,Type[][] elementTypes,string source,string replacement)
        {
            var result = _sut.Parse(input);
            AssertErrors(result);
            AssertAllTargetChains(explicatusChains, elementTypes, result, AktionType.Erstat);
        }

        private void AssertAllTargetChains(object[][] explicatusChains, Type[][] elementTypes, ParseResult<AendringDefinition> result, AktionType expectedAktionType)
        {
            Assert.Equal(explicatusChains.Length, result.Result.Targets.Length);

            int counter = 0;
            Assert.All(explicatusChains, chain =>
            {
                AendringDefinition parseResultResult = result.Result;
                AssertTargetElementChain(chain, expectedAktionType, elementTypes[counter], parseResultResult,
                    parseResultResult.Targets[counter]);
                counter++;
            });
        }

        

        [Theory]
        [InlineData("§ 15, stk. 2, 1. pkt, affattes således:", new object[] { 1,2, "15" }, new[] { typeof(Saetning), typeof(Stk), typeof(Paragraf) })]
        [InlineData("§ 15, stk. 2, affattes således:", new object[] { 2, "15" },  new[] { typeof(Stk), typeof(Paragraf) })]
        [InlineData("§ 153, stk. 1, ophæves, og i stedet indsættes:", new object[] { 1, "153" }, new Type[] { typeof(Stk), typeof(Paragraf) })]
        [InlineData("§ 198 b ophæves, og i stedet indsættes:", new object[] { "198 b" },  new []{ typeof(Paragraf) })]
        [InlineData("§ 9, stk. 1, nr. 1, litra d, affattes således:", new object[] { "d",1,1,"9" }, new[] { typeof(LitraOpregningElement), typeof(NummerOpregningElement), typeof(Stk),typeof(Paragraf) })]
        [InlineData("§ 98 e, stk. 4, 2. pkt., der bliver stk. 5, 2. pkt., affattes således:"
            , new object[] { 2, 5, "98 e" }
            , new[] { typeof(Saetning), typeof(Stk), typeof(Paragraf) })]
        public void Test_ErstatExpressions(string input, object[] expectedExplicatus,  Type[] expectedTypes)
        { 
            TestParseResult(input, expectedExplicatus, AktionType.Erstat, expectedTypes);
        }
        //§ 16, stk. 3-5, ophæves.

        [Theory]//§ 16, stk. 3-5, ophæves.
        [InlineData("§ 16, stk. 3-5, ophæves."
           , new object[] { new object[] { 3,16 }, new object[] { 4, 16 }, new object[] { 5, 16 } }
           , new object[] { new[] { typeof(Stk), typeof(Paragraf) }, new[] { typeof(Stk), typeof(Paragraf) }, new[] { typeof(Stk), typeof(Paragraf) } })]
        public void Test_MultiTarget_Ophaeves(string input, object[][] expectedExplicatus, Type[][] expectedTypes)
        {
            //Assert.True(true);
            var result = _sut.Parse(input);
            AssertErrors(result);
            AssertAllTargetChains(expectedExplicatus, expectedTypes, result, AktionType.Ophaev);
        }

        [Theory]
        [InlineData("§ 9 h, stk. 1 og 2, affattes således:"
           , new object[] { new object[] { 1, "9 h" }, new object[] { 2, "9 h" } }
           , new object[] { new[] { typeof(Stk), typeof(Paragraf) }, new[] { typeof(Stk), typeof(Paragraf) } })]
        public void Test_MultiTarget_Replace(string input, object[][] expectedExplicatus, Type[][] expectedTypes)
        {
            var result = _sut.Parse(input);
            AssertErrors(result);
            AssertAllTargetChains(expectedExplicatus,expectedTypes,result,AktionType.Erstat);
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
        [InlineData("I § 61, stk. 4, indsættes efter 1. pkt.:", new object[] {1,4,61}, new Type[] { typeof(Saetning),typeof(Stk), typeof(Paragraf) })]
        [InlineData("I § 17, stk. 1, indsættes efter nr. 1 som nye numre:", new object[] {1,1,17}, new Type[] { typeof(NummerOpregningElement),typeof(Stk), typeof(Paragraf) })]
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
            AssertErrors(parseResult);
            AendringDefinition parseResultResult = parseResult.Result;
            AssertTargetElementChain(expectedExplicatus, expectedAktionType, expectedTypes, parseResultResult, parseResultResult.Target);
        }

        private void AssertTargetElementChain(object[] expectedExplicatus, AktionType expectedAktionType,
            Type[] expectedType, AendringDefinition parseResultResult, Element elementTarget)
        {
            //Assert.Equal(Enumerable.Empty<string>(), parseResult.ErrorResult.Errors); //no errors
            Assert.Equal(expectedType.Count(), Enumerable.Count<Element>(elementTarget.GetAncestorsAndSelf)); //one target level
            Assert.Equal(expectedAktionType, parseResultResult.AktionType);

            //assert chain
            int counter = 0;
            Assert.All<Element>(elementTarget.GetAncestorsAndSelf, element =>
            {
                Assert.IsType(expectedType[counter], element);
                Assert.Equal(expectedExplicatus[counter].ToString(), element.Nummer.ToString());
                counter++;
            });
        }

        [InlineData("I stk. 10 indsættes efter »9607ee90-3697-4bae-8969-9091b5321«: »1234ee90-3697-4bae-8969-9091b5321«.", new object[] { 10 }, "9607ee90-3697-4bae-8969-9091b5321", "1234ee90-3697-4bae-8969-9091b5321",  new Type[] { typeof(Stk) })]
        [InlineData("I § 5 a, stk. 10, indsættes efter »9607ee90-3697-4bae-8969-9091b5321«: »1234ee90-3697-4bae-8969-9091b5321«.", new object[] { 10, "5 a" }, "9607ee90-3697-4bae-8969-9091b5321", "1234ee90-3697-4bae-8969-9091b5321", new Type[] { typeof(Stk), typeof(Paragraf) })]
        public void Test_Simple_SubelementTargetLevel_InsertAfter(string input, object[] expectedExplicatus, string quotedFrom,
            string quotedTo, Type[] aendringsType)
        {
            var parseResult = _sut.Parse(input);
            AssertErrors(parseResult);
            AendringDefinition parseResultResult = parseResult.Result;
            AssertTargetElementChain(expectedExplicatus, AktionType.IndsaetEfter, aendringsType, parseResultResult, parseResultResult.Target);
            AssertQuotedTextChange(quotedFrom, quotedTo, parseResult);
        }


        [Theory]
        //Nested citations not supported yet!!  [InlineData("I stk. 3 ændres »123»abcd«xyz« til: »9 8 7 6 5 4 3«.", new object[] {3} , "123»abcd«xyz", "9 8 7 6 5 4 3", AktionType.Erstat, new Type[] { typeof(Stk)})]
        [InlineData("I stk. 3 ændres »9607ee90-3697-4bae-8969-9091b5321« til: »1234ee90-3697-4bae-8969-9091b5321«.", new object[] {3} , "9607ee90-3697-4bae-8969-9091b5321", "1234ee90-3697-4bae-8969-9091b5321", new Type[] { typeof(Stk)})]
        [InlineData("I § 10, stk. 7, nr. 4, ændres »9607ee90-3697-4bae-8969-9091b5321« til: »1234ee90-3697-4bae-8969-9091b5321«.", new object[] {4,7,10}, "9607ee90-3697-4bae-8969-9091b5321", "1234ee90-3697-4bae-8969-9091b5321",  new Type[] {typeof(NummerOpregningElement), typeof(Stk),typeof(Paragraf)})]
        [InlineData("I § 448 b, 1. pkt., ændres »social- og indenrigsministeren eller den, ministeren bemyndiger dertil,« til: »Ankestyrelsen«.", new object[] { 1,"448 b"}, "social- og indenrigsministeren eller den, ministeren bemyndiger dertil,", "Ankestyrelsen", new Type[] { typeof(Saetning), typeof(Paragraf) })]
        //[InlineData("I § 82 a, stk. 3, der bliver stk. 4, ændres »stk. 1 og 2« til: »stk. 1-3«.", 
        //    new object[] { 3,"82 a"}, "stk. 1 og 2", "stk. 1-3", AktionType.Erstat, new Type[] { typeof(Stk), typeof(Paragraf) })]
        [InlineData("I § 3, ændres »101 kr.« til: »84 kr.«", new[] { "3" }, "101 kr.", "84 kr.",  new[] { typeof(Paragraf) })]
        //I § 82 a, stk. 3, der bliver stk. 4, ændres »stk. 1 og 2« til: »stk. 1-3«.
        [InlineData("I § 82 a, stk. 3, der bliver stk. 4, ændres »stk. 1 og 2« til: »stk. 1-3«.", new object[] { 4,"82 a" }, "stk. 1 og 2", "stk. 1-3",  new[] { typeof(Stk), typeof(Paragraf) })]
        

        public void TestSimple_SubelementTargetLevel_Replace(string input, object[] expectedExplicatus, string quotedFrom,
            string quotedTo,Type[] aendringsType)
        {
            var parseResult = _sut.Parse(input);
            AssertErrors(parseResult);
            AendringDefinition parseResultResult = parseResult.Result;
            AssertTargetElementChain(expectedExplicatus,AktionType.Erstat, aendringsType, parseResultResult, parseResultResult.Target);
            AssertQuotedTextChange(quotedFrom, quotedTo, parseResult);
        }

        private static void AssertQuotedTextChange(string quotedFrom, string quotedTo, ParseResult<AendringDefinition> parseResult)
        {
            Assert.Equal(quotedFrom, parseResult.Result.Target.SubElementTarget.Target);
            Assert.Equal(quotedTo, parseResult.Result.Target.SubElementTarget.Replacement);
        }

        [Theory]
        [InlineData("I § 9, stk. 1, udgår »§ 3 c, stk. 1, og«.", "§ 3 c, stk. 1, og", new object[] { 1, "9" },new[] {typeof(Stk),typeof(Paragraf)} )]
        public void TestQuotedTextRemoved_Simple(string input,string textToRemove,object[]explicatusChain,Type[]targetChainTypes)
        {
            var result = _sut.Parse(input);
            AssertErrors(result);
            AendringDefinition parseResultResult = result.Result;
            AssertTargetElementChain(explicatusChain,AktionType.Ophaev, targetChainTypes, parseResultResult, parseResultResult.Target);
            Assert.Equal(textToRemove,result.Result.Target.SubElementTarget.Target);
            Assert.Null(result.Result.Target.SubElementTarget.Replacement);
        }

        [Theory]
        [InlineData("I stk. 3 udgår »9607ee90-3697-4bae-8969-9091b5321«.", 3, "9607ee90-3697-4bae-8969-9091b5321", typeof(Stk))]
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
            Assert.All(parseResult.ErrorResult.Errors, e =>
            {
                Assert.Equal(string.Empty,e);
            });
            Assert.Empty(parseResult.ErrorResult.Errors);//no errors
        }
    }
}
