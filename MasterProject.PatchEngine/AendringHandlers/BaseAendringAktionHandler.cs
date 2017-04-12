using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition;

namespace MasterProject.PatchEngine.AendringHandlers
{
    public abstract class BaseAendringAktionHandler
    {
        private readonly AendringDefinition _aendringDefinition;
        protected readonly XElement AendringElement;

        protected BaseAendringAktionHandler(AendringDefinition aendringDefinition, XElement aendringElement)
        {
            _aendringDefinition = aendringDefinition;
            AendringElement = aendringElement;
        }
        

        protected IEnumerable<XElement> FindElementInTarget(TargetDocument targetDocument)
        {
            foreach (var target in _aendringDefinition.Targets)
            {
                var targetChain = target.GetAncestorsAndSelf.Reverse().ToArray();
                Element next = targetChain.First();
                XElement searchScope = targetDocument.Source.Root;
                while (next != null)
                {
                    var elementsFound = searchScope
                        .Descendants(next.LexdaniaName)
                        .Where(element => next.NummerMatch.IsMatch(element.Element("Explicatus")?.Value ?? string.Empty))
                        .ToArray();
                    ValidateFoundElements(elementsFound, next);
                    searchScope = elementsFound.First().AncestorsAndSelf(next.LexdaniaName).First();
                    var nextIndex = Array.IndexOf(targetChain, next) + 1;
                    next = nextIndex >= targetChain.Length ? null : targetChain[nextIndex];
                }
                yield return searchScope;
            }
            
        }

        private static void ValidateFoundElements(XElement[] elementsFound, Element next)
        {
            if (!elementsFound.Any())
                throw new ApplyAendringerException($"No elements found for change {next}");
            if (elementsFound.Length > 1)
                throw new ApplyAendringerException($"More than element found for change {next}");
        }
        
    }
}