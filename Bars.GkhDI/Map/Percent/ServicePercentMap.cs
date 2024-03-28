/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.GkhDi.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class ServicePercentMap : SubclassMap<ServicePercent>
///     {
///         public ServicePercentMap()
///         {
///             Table("DI_PERC_SERVICE");
///             KeyColumn("ID");
///             References(x => x.Service, "SERVICE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.ServicePercent"</summary>
    public class ServicePercentMap : JoinedSubClassMap<ServicePercent>
    {
        
        public ServicePercentMap() : 
                base("Bars.GkhDi.Entities.ServicePercent", "DI_PERC_SERVICE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Service, "Service").Column("SERVICE_ID").NotNull().Fetch();
        }
    }
}
