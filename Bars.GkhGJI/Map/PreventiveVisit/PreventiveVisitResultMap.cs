
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Приложения распоряжения ГЖИ"</summary>
    public class PreventiveVisitResultMap : BaseEntityMap<PreventiveVisitResult>
    {
        
        public PreventiveVisitResultMap() : 
                base("Приложения распоряжения ГЖИ", "GJI_PREVENTIVE_VISIT_RESULT")
        {
        }
        
        protected override void Map()
        {           
            Property(x => x.ProfVisitResult, "ProfVisitResult").Column("RESULT_TYPE").NotNull();
            Property(x => x.InformText, "Описание").Column("DESCRIPTION").Length(10000);
            Reference(x => x.PreventiveVisit, "Акт").Column("PREVENT_VISIT_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").Fetch();
        }
    }
}
