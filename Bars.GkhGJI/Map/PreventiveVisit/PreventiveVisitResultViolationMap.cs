
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Инспекторы документа ГЖИ"</summary>
    public class PreventiveVisitResultViolationMap : BaseEntityMap<PreventiveVisitResultViolation>
    {
        
        public PreventiveVisitResultViolationMap() : 
                base("Инспекторы документа ГЖИ", "GJI_PREVENTIVE_VISIT_RESULT_VIOLATION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PreventiveVisitResult, "Документ ГЖИ").Column("RESULT_ID").NotNull();
            Reference(x => x.ViolationGji, "ViolationGji").Column("VIOLATION_ID").NotNull().Fetch();
        }
    }
}
