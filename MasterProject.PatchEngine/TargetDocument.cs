using System.IO;
using System.Xml.Linq;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;

namespace MasterProject.PatchEngine
{
    public class TargetDocument
    {
        public DokumentType DokumentType { get;  }
        public int Year { get; }
        public int Number { get;  }
        private FileInfo _source;
        
        public TargetDocument(FileInfo source, DokumentType dokumentType, int year, int number)
        {
            _source = source;
            DokumentType = dokumentType;
            Year = year;
            Number = number;
            Source = XDocument.Load(source.FullName);
        }

        public XDocument Source { get; }
    }
}