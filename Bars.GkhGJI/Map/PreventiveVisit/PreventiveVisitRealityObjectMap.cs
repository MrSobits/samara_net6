
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Инспекторы документа ГЖИ"</summary>
    public class PreventiveVisitRealityObjectMap : BaseEntityMap<PreventiveVisitRealityObject>
    {
        
        public PreventiveVisitRealityObjectMap() : 
                base("Инспекторы документа ГЖИ", "GJI_PREVENTIVE_VISIT_RO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PreventiveVisit, "Документ ГЖИ").Column("PREVENT_VISIT_ID").NotNull();
            Reference(x => x.RealityObject, "MKD").Column("RO_ID").NotNull().Fetch();
        }
    }
}
