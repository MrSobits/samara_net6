/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Archangelsk.Map
/// {
///     using Bars.B4.DataAccess;
/// 
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Исполнитель обращения"
///     /// </summary>
///     public class AppealCitsExecutantMap : BaseEntityMap<AppealCitsExecutant>
///     {
///         public AppealCitsExecutantMap()
///             : base("GJI_APPCIT_EXECUTANT")
///         {
///             Map(x => x.OrderDate, "ORDER_DATE");
///             Map(x => x.PerformanceDate, "PERFOM_DATE");
///             Map(x => x.IsResponsible, "RESPONSIBLE");
///             Map(x => x.Description, "DESCRIPTION").Length(255);
/// 
///             References(x => x.AppealCits, "APPCIT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Executant, "EXECUTANT_ID").Fetch.Join();
///             References(x => x.Author, "AUTHOR_ID").Fetch.Join();
///             References(x => x.State, "STATE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Archangelsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Archangelsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Archangelsk.Entities.AppealCitsExecutant"</summary>
    public class AppealCitsExecutantMap : BaseEntityMap<AppealCitsExecutant>
    {
        
        public AppealCitsExecutantMap() : 
                base("Bars.GkhGji.Regions.Archangelsk.Entities.AppealCitsExecutant", "GJI_APPCIT_EXECUTANT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OrderDate, "OrderDate").Column("ORDER_DATE");
            Property(x => x.PerformanceDate, "PerformanceDate").Column("PERFOM_DATE");
            Property(x => x.IsResponsible, "IsResponsible").Column("RESPONSIBLE");
            Property(x => x.Description, "Description").Column("DESCRIPTION").Length(255);
            Reference(x => x.AppealCits, "AppealCits").Column("APPCIT_ID").NotNull().Fetch();
            Reference(x => x.Executant, "Executant").Column("EXECUTANT_ID").Fetch();
            Reference(x => x.Author, "Author").Column("AUTHOR_ID").Fetch();
            Reference(x => x.State, "State").Column("STATE_ID").Fetch();
        }
    }
}
