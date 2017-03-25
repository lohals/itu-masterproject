using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Dk.Itu.Rlh.MasterProject.Model.AendringsDefinition
{
    public class ModelSerializer
    {
        private IEnumerable<Type> _knownElementTypes;

        public ModelSerializer()
        {
            _knownElementTypes = this.GetType().Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Element)) && !type.IsGenericType);
        }

        public string Serialize(AendringDefinition input)
        {
            var serializer = new XmlSerializer(typeof(AendringDefinition), _knownElementTypes.ToArray());
            using (var mem = new MemoryStream())
            {
                serializer.Serialize(mem, input);
                mem.Position = 0;
                return Encoding.UTF8.GetString(mem.ToArray());

            }
        }
    }
}