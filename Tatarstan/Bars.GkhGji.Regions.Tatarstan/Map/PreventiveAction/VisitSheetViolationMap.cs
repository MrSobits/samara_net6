namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// Маппинг <see cref="VisitSheetViolation"/>
    /// </summary>
    public class VisitSheetViolationMap : BaseEntityMap<VisitSheetViolation>
    {
        /// <inheritdoc />
        public VisitSheetViolationMap()
            : base(nameof(VisitSheetViolation), "GJI_VISIT_SHEET_VIOLATION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.ViolationInfo, nameof(VisitSheetViolation.ViolationInfo)).Column("VIOLATION_INFO_ID");
            this.Reference(x => x.Violation, nameof(VisitSheetViolation.Violation)).Column("VIOLATION_ID");
            this.Property(x => x.IsThreatToLegalProtectedValues, nameof(VisitSheetViolation.IsThreatToLegalProtectedValues)).Column("IS_THREAT_TO_LEGAL_PROTECTED_VALUES");
        }
    }
}