namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Обращениям граждан - Запрос"</summary>
    public class AppealCitsExecutionTypeMap : BaseEntityMap<AppealCitsExecutionType>
    {
        
        public AppealCitsExecutionTypeMap() : 
                base("Обращения граждан - Исполнение", "GJI_APPCIT_EXECUTION_TYPE")
        {
        }
        
        protected override void Map()
        {           
            Reference(x => x.AppealCits, "AppealCits").Column("APPCIT_ID").NotNull().Fetch();
            Reference(x => x.AppealExecutionType, "AppealExecutionType").Column("TYPE_ID").NotNull().Fetch();
        }
    }
    
}
