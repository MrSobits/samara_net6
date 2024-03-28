namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.DisposalControlMeasures"</summary>
    public class DecisionControlListMap : BaseEntityMap<DecisionControlList>
    {
        
        public DecisionControlListMap() : 
                base("Bars.GkhGji.Entities", "GJI_DECISION_CON_LIST")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Decision, "Disposal").Column("DECISION_ID").NotNull();
            Reference(x => x.ControlList, "ControlList").Column("CONTROL_LIST_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }
}
