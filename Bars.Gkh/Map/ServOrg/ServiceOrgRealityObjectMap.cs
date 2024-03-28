/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.ManOrg
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Жилой дом организации поставщика жил. услуг"
///     /// </summary>
///     public class ServiceOrgRealityObjectMap : BaseGkhEntityMap<ServiceOrgRealityObject>
///     {
///         public ServiceOrgRealityObjectMap()
///             : base("GKH_SERV_ORG_REAL_OBJ")
///         {
///             References(x => x.ServiceOrg, "SERV_ORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Жилой дом организации поставщика жил. услуг"</summary>
    public class ServiceOrgRealityObjectMap : BaseImportableEntityMap<ServiceOrgRealityObject>
    {
        
        public ServiceOrgRealityObjectMap() : 
                base("Жилой дом организации поставщика жил. услуг", "GKH_SERV_ORG_REAL_OBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ServiceOrg, "Организация поставщик жил. услуг").Column("SERV_ORG_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
