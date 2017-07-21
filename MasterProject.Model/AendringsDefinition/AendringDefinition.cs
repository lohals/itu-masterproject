using System;
using System.Linq;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    [Serializable]
    public class AendringDefinition
    {
        public Element[] Targets { get; set; }
        public Element Target => Targets?.FirstOrDefault();
        public AktionType AktionType { get; set; }
        public Element[] StructureTargets => Targets?.Where(element => element.IsStructureElement).ToArray();
    }
}