namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.DisposalControlMeasures"</summary>
    public class DecisionInspectionReasonMap : BaseEntityMap<DecisionInspectionReason>
    {
        
        public DecisionInspectionReasonMap() : 
                base("Bars.GkhGji.Entities", "GJI_DECISION_DEC_REASON")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Decision, "Disposal").Column("DECISION_ID").NotNull();
            Reference(x => x.InspectionReason, "InspectionReason").Column("DEC_REASON_ID").NotNull().Fetch();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }
}
