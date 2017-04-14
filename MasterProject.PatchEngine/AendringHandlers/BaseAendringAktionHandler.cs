using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace MasterProject.PatchEngine.AendringHandlers
{
    public abstract class BaseAendringAktionHandler
    {
        protected readonly AendringDefinition AendringDefinition;
        protected readonly XElement AendringElement;

        protected BaseAendringAktionHandler(AendringDefinition aendringDefinition, XElement aendringElement)
        {
            AendringDefinition = aendringDefinition;
            AendringElement = aendringElement;
        }

        protected IEnumerable<XElement> FindStructureElementsInTargetDocument(TargetDocument targetDocument, Element targetsToLookFor)
        {
            return FindStructureElementsInTargetDocument(targetDocument, new[] {targetsToLookFor});
        }

        protected IEnumerable<XElement> FindStructureElementsInTargetDocument(TargetDocument targetDocument, IEnumerable<Element> targetsToLookFor)
        {
            foreach (var target in targetsToLookFor)
            {
                var targetChain = target.GetAncestorsAndSelf.Reverse().ToArray();
                Element next = targetChain.First();
                XElement searchScope = targetDocument.Source.Root;
                bool success = false;
                while (next != null)
                {
                    var elementsFound = searchScope
                        .Descendants(next.LexdaniaName)
                        .Where(element => next.NummerMatch.IsMatch(element.Element("Explicatus")?.Value ?? string.Empty) || IsFirstStkMatch(next, element))
                        .ToArray();
                    ValidateFoundElements(elementsFound, next);
                    if (!elementsFound.Any())
                        break;
                    searchScope = elementsFound.First().AncestorsAndSelf(next.LexdaniaName).First();
                    var nextIndex = Array.IndexOf(targetChain, next) + 1;
                    next = nextIndex >= targetChain.Length ? null : targetChain[nextIndex];
                    if (targetChain.Length <= nextIndex)
                        success = true;
                }
                if (success)
                    yield return searchScope;
            }
            
        }

        private bool IsFirstStkMatch(Element nummer, XElement nextNummer)
        {
            var stk = nummer as Stk;
            if (stk?.NummerStrong != 1)
                return false;
            return nextNummer.Parent.Element(stk.LexdaniaName).Equals(nextNummer);
        }

        private static void ValidateFoundElements(XElement[] elementsFound, Element next)
        {
            //if (!elementsFound.Any())
            //    throw new ApplyAendringerException($"No elements found for change {next}");
            if (elementsFound.Length > 1)
                throw new ApplyAendringerException($"More than element found for change {next}");
        }

        protected static XElement SelectSameTypeSelfOrImmediateAncestor(XElement initialTargetElement, IEnumerable<XElement> elementToInsert)
        {
            var xName = initialTargetElement.Name;
            XElement siblingToInsertAfter = initialTargetElement;
            var xElements = elementToInsert.ToArray();
            var elementsCommonElementName = xElements.First().Name;
            if (xName != elementsCommonElementName)
                siblingToInsertAfter = initialTargetElement.Ancestors(elementsCommonElementName).First();
            return siblingToInsertAfter;
        }

        protected IEnumerable<XElement> GetAendringAktionStructures()
        {
            return AendringElement.Element("AendringAktion").Element("AendringNyTekst").Elements();
        }

        protected IEnumerable<Saetning> GetSupportedSubStructureTargets()
        {
            return AendringDefinition.Targets.Where(element => !element.IsStructureElement).OfType<Saetning>();
        }

        protected XElement[] GetTargetStructureForSubstructureChange(TargetDocument targetDocument, Saetning saetning)
        {
            var targetStructure = saetning.GetAncestorsAndSelf.FirstOrDefault(e => e.IsStructureElement);

            var structureElementsFound = FindStructureElementsInTargetDocument(targetDocument, targetStructure).ToArray();
            return structureElementsFound;
        }
    }
}