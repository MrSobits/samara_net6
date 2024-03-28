namespace Bars.GkhGji.Report.Form123Extended
{
    using Bars.GkhGji.Enums;

    internal sealed class ActCheckProxy : IDocGjiForm123ExtReport
    {
        public long Id { get; set; }
        public TypeBase InspectionTypeBase { get; set; }

        public decimal? Area;
        public long InspectionContragentId;
    }
}
