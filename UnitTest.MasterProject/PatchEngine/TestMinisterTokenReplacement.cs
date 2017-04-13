using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest.Dk.Itu.Rlh.MasterProject.PatchEngine
{
    public class TestMinisterTokenReplacement
    {
        [Theory]
        [InlineData("dsaf asdf Ministeren for fødevarer, landbrug og fiskeri nedsætter", "Ministeren for fødevarer, landbrug og fiskeri")]
        [InlineData("fdkf df Miljø- og fødevareministeren kan","Miljø- og fødevareministeren")]
        [InlineData("Ministeren for fødevarer, landbrug og fiskeri kan fastsætte regler eller ", "Ministeren for fødevarer, landbrug og fiskeri")]
        [InlineData("Anmodning herom skal fremsættes over for miljø- og fødevareministeren, kan inden 4 uger efter at tilbagekaldelsen er meddelt autorisationsindehaveren.", "miljø- og fødevareministeren")]
        [InlineData("når autorisationsindehaveren til ministeren for fødevarer, landbrug og fiskeri eller den, ministeren bemyndiger hertil, meddeler ophør af produktionen.", "ministeren for fødevarer, landbrug og fiskeri")]
        [InlineData("Hvor det efter denne lov eller regler udstedt i medfør af denne lov er krævet, at et dokument, som er udstedt af andre end miljø- og fødevareministeren, skal være underskrevet, kan dette krav opfyldes ved anvendelse af en teknik, der sikrer entydig identifikation af den, som har udstedt dokumentet, jf. dog stk. 2.", "miljø- og fødevareministeren")]
        [InlineData("Henlægger miljø- og fødevareministeren sine beføjelser efter loven til en myndighed under ministeriet, kan ministeren fastsætte regler om adgangen til at klage over myndighedens afgørelser, herunder om, at klage ikke kan indbringes for en anden administrativ myndighed, og om myndighedens adgang til at genoptage en sag, efter at der er indgivet klage.", "miljø- og fødevareministeren")]
        [InlineData("Skønnes en overtrædelse ikke at ville medføre højere straf end bøde, kan miljø- og fødevareministeren tilkendegive, at sagen kan afgøres uden retslig forfølgning, " +
                    "hvis den, der har begået overtrædelsen, erklærer sig skyldig i overtrædelsen og rede til inden en nærmere angiven frist, der efter begæring kan forlænges, at betale en i tilkendegivelsen angivet bøde.", "miljø- og fødevareministeren")]
        public void TestThatMinisterReferenceRegex(string source, string expected)
        {
            var result = MinisterTokenReplacer.FindMinisterMatches(source);
            Assert.Equal(expected,result.Value);
        }
        [Theory]
        [InlineData("kan fastsætte regler om, at skriftlig kommunikation til og fra Miljø- og Fødevareministeriet om forhold, som er omfattet af denne lov eller af regler udstedt i medfør af denne lov, skal foregå digitalt.", "Miljø- og Fødevareministeriet")]
        [InlineData("kan fastsætte regler om, at skriftlig kommunikation til og fra Ministeriet for Fødevarer, Landbrug og Fiskeri om forhold, som er omfattet af denne lov eller af regler udstedt i medfør af denne lov, skal foregå digitalt.", "Ministeriet for Fødevarer, Landbrug og Fiskeri")]
        public void TestThatMinisterieReferenceRegex(string source, string expected)
        {
            var result = MinisterTokenReplacer.FindMinisterieMatches(source);
            Assert.Equal(expected, result.Value);
        }
    }
}
