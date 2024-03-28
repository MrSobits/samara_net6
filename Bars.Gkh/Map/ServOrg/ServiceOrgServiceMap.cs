/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Услуга управляющей организации"
///     /// </summary>
///     public class ServiceOrgServiceMap : BaseGkhEntityMap<ServiceOrgService>
///     {
///         public ServiceOrgServiceMap() : base("GKH_SERV_ORG_SERVICE")
///         {
///             References(x => x.ServiceOrganization, "SERV_ORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.TypeService, "TYPE_SERVICE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Услуга обслуживающей организации"</summary>
    public class ServiceOrgServiceMap : BaseImportableEntityMap<ServiceOrgService>
    {
        
        public ServiceOrgServiceMap() : 
                base("Услуга обслуживающей организации", "GKH_SERV_ORG_SERVICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ServiceOrganization, "Обслуживающая организация").Column("SERV_ORG_ID").NotNull().Fetch();
            Reference(x => x.TypeService, "Тип обслуживания").Column("TYPE_SERVICE_ID").NotNull().Fetch();
        }
    }
}
