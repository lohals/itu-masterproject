using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;

namespace Dk.Itu.Rlh.MasterProject.Model
{
    public class Dokument
    { 
         public string Title { get; set; }
         public DokumentReferenceData DokumentReference { get; set; }

    }
}