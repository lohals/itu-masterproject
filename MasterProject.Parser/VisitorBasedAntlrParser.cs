﻿
using Antlr4.Runtime;
using Dk.Itu.Rlh.MasterProject.Model;
using Dk.Itu.Rlh.MasterProject.Grammar;
using System;
using Antlr4.Runtime.Tree;
using MasterProject.Utilities;

namespace Dk.Itu.Rlh.MasterProject.Parser
{

    public abstract class VisitorBasedAntlrParser<T>
    {

        public ParseResult<T> Parse(string inputString)
        {
            var cleanedString = inputString.NormalizeWhiteSpace();
            var input = new AntlrInputStream(cleanedString);
            var lexer = GetLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var errorListener = new ParserErrorListener();
            var tree = GetParseTree(tokens, errorListener);
            var visitor = GetVisitor();
            return new ParseResult<T>()
            {
                Result = visitor.Visit(tree),
                ErrorResult = errorListener.ErrorResult
            };
        }

        protected abstract IParseTree GetParseTree(CommonTokenStream tokens, BaseErrorListener errorListener);

        protected abstract AbstractParseTreeVisitor<T> GetVisitor();

        protected abstract ITokenSource GetLexer(ICharStream input);       
        

    }
}
