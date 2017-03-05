using System;
using System.Xml.Serialization;

namespace Dk.Itu.Rlh.MasterProject.Model
{
    [Serializable]
    public partial class AendringDefinition
    {
        
        public Element Target { get; set; }
        public AktionType AktionType { get; set; }
    }
}