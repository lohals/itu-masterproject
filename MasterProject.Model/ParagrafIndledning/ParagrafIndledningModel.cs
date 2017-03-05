using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning
{
    public class ParagrafIndledningModel
    {
        public IList<Dokument> References { get; set; } = new List<Dokument>();
    }
}
