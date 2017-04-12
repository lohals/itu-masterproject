using System;
using System.IO;
using System.Xml.Linq;

namespace MasterProject.PatchEngine
{
    public class ChangeDocument
    {
        public int Year { get; }
        public int Number { get; }

        private readonly FileInfo _fileInfo;

        public ChangeDocument(FileInfo fileInfo, int year, int number)
        {
            Number = number;
            _fileInfo = fileInfo;
            Year = year;
        }
        public XDocument XDoc => XDocument.Load(_fileInfo.FullName);

    }
}