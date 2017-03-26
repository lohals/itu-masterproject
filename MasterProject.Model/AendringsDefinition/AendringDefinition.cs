using System;
using System.Linq;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    [Serializable]
    public partial class AendringDefinition
    {
        public Element[] Targets { get; set; }
        public Element Target => Targets?.FirstOrDefault();
        public AktionType AktionType { get; set; }
    }
}