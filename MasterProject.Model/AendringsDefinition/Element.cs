using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    [Serializable]
    public abstract class Element
	{
        public abstract object Nummer { get;  }

        [XmlIgnore]
        public SubElementTarget SubElementTarget =>SubElementTargets?.FirstOrDefault();

        [XmlElement(Order = 2)]
        public SubElementTarget[] SubElementTargets { get; set; }


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
        public abstract Element Clone(object nummer);

    }


    public abstract class Element<T>:Element
    {
        
        public override object Nummer 
        {
            get { return NummerStrong; }
        }

        public override Element Clone(object nummer)
        {
            return CloneStrong((T)nummer);
        }


        [XmlAttribute(AttributeName = "Nummer")]
        public T NummerStrong { get; set; }
        public abstract Element<T> CloneStrong(T nummer);


    }
}