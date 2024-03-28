namespace Bars.GkhGji.Report.Form123Extended
{
    using Bars.GkhGji.Enums;

    internal sealed class DisposalProxy : IDocGjiForm123ExtReport
    {
        public long Id { get; set; }
        public TypeBase InspectionTypeBase { get; set; }

        public TypeCheck KindCheckCode;
        public PersonInspection InspectionPersonInspection;
        public TypeDisposalGji TypeDisposal;
        public long stageId;
    }
}
