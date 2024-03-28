namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.DisposalControlMeasures"</summary>
    public class DisposalControlMeasuresMap : BaseEntityMap<DisposalControlMeasures>
    {
        
        public DisposalControlMeasuresMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.DisposalControlMeasures", "GJI_KHAKASIA_DISP_CON_MEASURE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Disposal, "Disposal").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.ControlActivity, "ControlActivity").Column("CONTROL_MEASURES_ID").NotNull().Fetch();
        }
    }
}
