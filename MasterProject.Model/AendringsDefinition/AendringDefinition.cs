using System;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    [Serializable]
    public partial class AendringDefinition
    {
        
        public Element Target { get; set; }
        public AktionType AktionType { get; set; }
    }
}