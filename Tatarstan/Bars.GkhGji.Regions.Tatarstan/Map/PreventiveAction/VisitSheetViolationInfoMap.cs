namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// Маппинг <see cref="VisitSheetViolationInfo"/>
    /// </summary>
    public class VisitSheetViolationInfoMap : BaseEntityMap<VisitSheetViolationInfo>
    {
        /// <inheritdoc />
        public VisitSheetViolationInfoMap()
            : base(nameof(VisitSheetViolationInfo), "GJI_VISIT_SHEET_VIOLATION_INFO")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.VisitSheet, nameof(VisitSheetViolationInfo.VisitSheet)).Column("VISIT_SHEET_ID");
            this.Reference(x => x.RealityObject, nameof(VisitSheetViolationInfo.RealityObject)).Column("REALITY_OBJECT_ID");
            this.Property(x => x.Description, nameof(VisitSheetViolationInfo.Description)).Column("DESCRIPTION");
        }
    }
}