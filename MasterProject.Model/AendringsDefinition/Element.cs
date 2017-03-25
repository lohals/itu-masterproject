using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    [Serializable]
    public abstract class Element
	{

        public abstract object Nummer { get;  }

        [XmlElement(Order = 2)]
        public SubElementTarget SubElementTarget { get; set; }

        [XmlElement(Order = 1)]
        public Element ParentContext { get; set; }
        public IEnumerable<Element> GetAncestorsAndSelf
        { 
            get {
                var currentParent = this;
                while (currentParent!=null)
                {
                    var objetToReturn = currentParent;
                    currentParent = objetToReturn.ParentContext;
                    yield return objetToReturn;
                }
            } 
        }
	}

    public abstract class Element<T>:Element
    {
        
        public override object Nummer 
        {
            get { return NummerStrong; }
        }

        [XmlAttribute(AttributeName = "Nummer")]
        public T NummerStrong { get; set; }
        
    }
}