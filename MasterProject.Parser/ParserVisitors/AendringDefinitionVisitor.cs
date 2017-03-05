using Dk.Itu.Rlh.MasterProject.Grammar;
using Dk.Itu.Rlh.MasterProject.Model;
using System.Collections.Generic;
using System.Linq;

namespace Dk.Itu.Rlh.MasterProject.Parser.ParserVisitors
{
    public class AendringDefinitionVisitor: AendringDefinitionGrammarBaseVisitor<AendringDefinition>
    {
        public override AendringDefinition VisitInsertLastExp(AendringDefinitionGrammarParser.InsertLastExpContext context)
        {
            //Check for context
            Element parentContext = context.elementChainExp()?.Accept(new ElementChainVisitor());
          
            //get special last element
            var last = context.lastElementExp()?.Accept(new ElementVisitor());
            if (last == null)
                return null;
            var nummerElement = last as Element<int>;
            if (nummerElement != null)
            {
                nummerElement.NummerStrong--;
            }

            if (parentContext!=null)
                last.ParentContext = parentContext;

            return BuildAendringDefintion(last,
                AktionType.IndsaetEfter);
        }

        public override AendringDefinition VisitParentTargetChangedExp(AendringDefinitionGrammarParser.ParentTargetChangedExpContext context)
        {
            return BuildAendringDefintion(new ParentElementContext(), AktionType.Erstat);
        }

        public override AendringDefinition VisitParentTargetRemovedExp(AendringDefinitionGrammarParser.ParentTargetRemovedExpContext context)
        {
            return BuildAendringDefintion(new ParentElementContext(), AktionType.Ophaev);
        }

        public override AendringDefinition VisitInsertAfterChainExp(AendringDefinitionGrammarParser.InsertAfterChainExpContext context)
        {
            var target = GetTargetElement(context);
            if (context.quotedTextChangeExp() != null)
                target.SubElementTarget = context.quotedTextChangeExp().Accept(SubElementTargetVisitor.NewInstance);
            return BuildAendringDefintion(target, AktionType.IndsaetEfter);
        }

        public override AendringDefinition VisitInsertAfterExp(AendringDefinitionGrammarParser.InsertAfterExpContext context)
        {
            if (context.elementExp() != null)
                return BuildAendringDefintion(context.elementExp().Accept(new ElementVisitor()),AktionType.IndsaetEfter);
            if (context.aendringsNummerExp() != null)
                return BuildAendringDefintion(context.aendringsNummerExp().Accept(new ElementVisitor()),
                    AktionType.IndsaetEfter);
            return null;
        }

        private Element GetTargetElement(AendringDefinitionGrammarParser.InsertAfterChainExpContext context)
        {
            //return null;
            var firstContext = context.elementChainExp()?.Accept(new ElementChainVisitor());
            var lastContext = context.lastElementExp()?.Accept(new ElementVisitor());
            var chain = new[] {firstContext,lastContext}.Where(element => element!=null).ToArray();
            return SetupParentChain(chain);
        }

        private Element SetupParentChain(IList<Element> elementContext)
        {
            if (!elementContext.Any())
                return null;
            var currentParent = elementContext.FirstOrDefault();
            foreach (var element in elementContext.Skip(1).ToArray())
            {
                //Chain previous inner target to current chains outermost element
                var localChain = element.GetAncestorsAndSelf.ToArray();
                localChain.LastOrDefault().ParentContext = currentParent;
                currentParent = localChain.FirstOrDefault();
            }
            return elementContext.LastOrDefault();
        }


        public override AendringDefinition VisitInsertBeforeExp(AendringDefinitionGrammarParser.InsertBeforeExpContext context)
        {
            var target = SetupParentChain(
                new[]
                {
                    context.elementChainExp()?.Accept(new ElementChainVisitor())
                }.Concat(
                    new []
                    {
                        context.elementOrOpregningExp()?.Accept(new ElementVisitor())
                    })
                    .Where(element => element!=null)
                    .ToArray());

            return BuildAendringDefintion(target, AktionType.IndsaetFoer);
        }
        private AendringDefinition BuildAendringDefintion(Element target, AktionType aktionType)
        {
            return new AendringDefinition() { Target = target,AktionType =aktionType };
        }
        public override AendringDefinition VisitRemoveExp(AendringDefinitionGrammarParser.RemoveExpContext context)
        {
            var target = context.elementChainExp()?.Accept(new ElementChainVisitor());
            if (target !=null && context.QUOTEDTEXT() != null)
                target.SubElementTarget = new SubElementTarget() { Target = context.QUOTEDTEXT().GetText() };
            return BuildAendringDefintion(target, AktionType.Ophaev);
        }

        public override AendringDefinition VisitManualExp(AendringDefinitionGrammarParser.ManualExpContext context)
        {
            var targets = context.elementChainExp()?.Accept(new ElementChainVisitor());
            return BuildAendringDefintion(targets,AktionType.Manuel);
        }


        public IList<Element> GetElements(IReadOnlyList<AendringDefinitionGrammarParser.ElementExpContext> elementContext)
        {
            var targets = elementContext
                .Select(exp => exp?.Accept(ElementVisitor.NewInstance))
                .Where(exp=>exp!=null).ToList();

            Element currentParent = null;

            foreach (var item in targets)
            {
                if (currentParent != null)
                    item.ParentContext = currentParent;
                currentParent = item;
            }
            return targets;
        }
        public override AendringDefinition VisitReplaceExp(AendringDefinitionGrammarParser.ReplaceExpContext context)
        {
            var aendringsNummer = context.aendringsNummerExp()?.Accept(ElementVisitor.NewInstance);

            var elementChain = context.elementChainExp()?
                .Select(expContext => expContext?.Accept(new ElementChainVisitor()))
                .Where(element => element!=null)
                .ToArray();

            var innerElementTarget = SetupParentChain(new []
            {
                aendringsNummer
            }
            .Concat(elementChain)
            .Where(element => element!=null)
            .ToArray());

            if (innerElementTarget == null)
                return null;
            
            innerElementTarget.SubElementTarget = context.quotedTextChangeExp()?.Accept(SubElementTargetVisitor.NewInstance);

            return BuildAendringDefintion(innerElementTarget, AktionType.Erstat);
        }
        

        public override AendringDefinition VisitAendringDefinition(AendringDefinitionGrammarParser.AendringDefinitionContext context)
        {
            return context.phrase().Accept(this);
        }
    }
}
