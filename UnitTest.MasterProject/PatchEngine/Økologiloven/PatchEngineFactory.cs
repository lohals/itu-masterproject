﻿using System.Linq;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine.Økologiloven
{
    public class PatchEngineFactory
    {
        private static IPatchTask[] _patchTask;

        public PatchEngineFactory()
        {
            _patchTask=new IPatchTask[]
            {
                new SchemaPatch("http://www.retsinformation.dk/offentlig/xml/schemas/2011/03/21/LBKH.Retsinfo.LexDania_2.1.xsd"),
                new TitelBuilder(),
                new PatchHymne(), 
                new IndledningBuilder()
            };
        }
        public FileBasedPatchEngine Create()
        {
            return new FileBasedPatchEngine(_patchTask);
        }
    }
}