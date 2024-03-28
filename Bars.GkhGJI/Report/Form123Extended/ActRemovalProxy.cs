namespace Bars.GkhGji.Report.Form123Extended
{
    using Bars.GkhGji.Enums;

    public class ActRemovalProxy : IDocGjiForm123ExtReport
    {
        public long Id { get; set; }
        public TypeBase InspectionTypeBase { get; set; }

        public decimal? Area;
        public long parentStage;
    }
}