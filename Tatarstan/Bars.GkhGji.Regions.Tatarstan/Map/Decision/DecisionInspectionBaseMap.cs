namespace Bars.GkhGji.Regions.Tatarstan.Map.Decision
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    /// <summary>
    /// Маппинг сущности <see cref="DecisionInspectionBase"/>
    /// </summary>
    public class DecisionInspectionBaseMap : BaseEntityMap<DecisionInspectionBase>
    {
        /// <inheritdoc />
        public DecisionInspectionBaseMap()
            : base("Основания проведения", "GJI_DECISION_INSPECTION_BASE")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.InspectionBaseType, "InspectionBaseType").Column("INSP_BASE_TYPE_ID").NotNull();
            this.Reference(x => x.Decision, "Decision").Column("DECISION_ID").NotNull();
            this.Property(x => x.OtherInspBaseType, "OtherInspBaseType").Column("OTHER_INSP_BASE_TYPE");
            this.Property(x => x.FoundationDate, "FoundationDate").Column("FOUNDATION_DATE");
            this.Reference(x => x.RiskIndicator, "RiskIndicator").Column("RISK_INDICATOR_ID");
            this.Property(x => x.ErknmGuid, "ErknmGuid").Column("ERKNM_GUID").Length(36);
        }
    }
}