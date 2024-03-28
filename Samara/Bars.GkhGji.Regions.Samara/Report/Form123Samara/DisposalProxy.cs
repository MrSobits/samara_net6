namespace Bars.GkhGji.Regions.Samara.Report.Form123Samara
{
    using System;

    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Report;

    internal sealed class DisposalProxy : IEquatable<DisposalProxy>
    {
        public long Id { get; set; }
        public TypeCheck KindCheckCode { get; set; }
        public TypeBase InspectionTypeBase { get; set; }
        public PersonInspection InspectionPersonInspection { get; set; }
        public TypeDisposalGji TypeDisposal { get; set; }
        public long StageId { get; set; }

        public bool Equals(DisposalProxy other)
        {
            if (Object.ReferenceEquals(other, null)) { return false; }
            if (Object.ReferenceEquals(this, other)) { return true; }

            var res = this.Id == other.Id 
                && this.KindCheckCode == other.KindCheckCode
                && this.InspectionTypeBase == other.InspectionTypeBase
                && this.TypeDisposal == other.TypeDisposal
                && this.StageId == other.StageId;
            return res;
        }

        public override int GetHashCode()
        {
            int hashId = this.Id.GetHashCode();
            int hashKindCheckCode = this.KindCheckCode.GetHashCode();
            int hashInspectionTypeBase = this.InspectionTypeBase.GetHashCode();
            int hashInspectionPersonInspection = this.InspectionPersonInspection.GetHashCode();
            int hashTypeDisposal = this.TypeDisposal.GetHashCode();
            int hashStageId = this.StageId.GetHashCode();

            return hashId ^ hashKindCheckCode ^ hashInspectionTypeBase ^ hashInspectionPersonInspection ^ hashTypeDisposal ^ hashStageId;
        }
    }
}
