namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.DisposalControlMeasures"</summary>
    public class DecisionControlMeasuresMap : BaseEntityMap<DecisionControlMeasures>
    {
        
        public DecisionControlMeasuresMap() : 
                base("Bars.GkhGji.Entities", "GJI_DECISION_CON_MEASURE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Decision, "Disposal").Column("DECISION_ID").NotNull().Fetch();
            Reference(x => x.ControlActivity, "ControlActivity").Column("CONTROL_MEASURES_ID").NotNull().Fetch();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
        }
    }
}
