using Dk.Itu.Rlh.MasterProject.Model.ParagrafIndledning;

namespace Dk.Itu.Rlh.MasterProject.Model
{
    public static class DokumentTypeHelpers
    {
        public static DokumentType MapShortNameToDokumentType(string value)
        {
            switch (value.ToLower())
            {
                case "lovh":
                    return DokumentType.Lov;
                case "lbkh":
                    return DokumentType.LovBekendtg�relse;
                case "lovc":
                    return DokumentType.Lov�ndring;
                default:
                    return DokumentType.Unknown;
            }
        }
    }
}