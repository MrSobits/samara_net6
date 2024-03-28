namespace Bars.GkhGji.Report.Form123Extended
{
    using Bars.GkhGji.Enums;

    internal sealed class PrescriptionProxy : IDocGjiExecutantCodeForm123ExtReport
    {
        public long Id { get; set; }
        public TypeBase InspectionTypeBase { get; set; }
        public string ExecutantCode { get; set; }

        public long parentStage;
    }
}
