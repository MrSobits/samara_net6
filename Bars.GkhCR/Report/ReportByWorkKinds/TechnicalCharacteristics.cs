namespace Bars.GkhCr.Report.ReportByWorkKinds
{
    using Bars.Gkh.Enums;

    public struct TechnicalCharacteristics
    {
        public int? Storeys { get; set; }

        public decimal? TotalArea { get; set; }

        public int? FlatsNum { get; set; }

        public int? CitizensNum { get; set; }

        public TypeRoof TypeRoof { get; set; }
    }
}