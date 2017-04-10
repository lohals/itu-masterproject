using System;
using System.IO;
using System.Xml.Linq;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine.Økologiloven
{
    public class ChangeDocument
    {
        public System.DateTime UnderskriftDato { get; }
        public int Year { get; }
        public int Number { get; }

        private readonly FileInfo _fileInfo;

        public ChangeDocument(FileInfo fileInfo, DateTime underskriftDao, int year, int number)
        {
            UnderskriftDato = underskriftDao;
            Number = number;
            _fileInfo = fileInfo;
            Year = year;
        }
        public XDocument XDoc => XDocument.Load(_fileInfo.FullName);

    }
}