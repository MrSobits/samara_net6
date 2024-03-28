/// <mapping-converter-backup>
/// namespace Bars.Gkh1468.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh1468.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности  "Связь Поставщика ресурсов с Жилым домом"
///     /// </summary>
///     public class PublicServOrgRealtyObjectMap : BaseEntityMap<PublicServiceOrgRealtyObject>
///     {
///         public PublicServOrgRealtyObjectMap()
///             : base("GKH_PUBLIC_SERVORG_RO")
///         {
///             References(x => x.PublicServiceOrg, "PUBLIC_SERVORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITYOBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh1468.Entities;
    
    
    /// <summary>Маппинг для "Связь Поставщик ресурсов с жилым домом"</summary>
    public class PublicServiceOrgRealtyObjectMap : BaseEntityMap<PublicServiceOrgRealtyObject>
    {
        
        public PublicServiceOrgRealtyObjectMap() : 
                base("Связь Поставщик ресурсов с жилым домом", "GKH_PUBLIC_SERVORG_RO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PublicServiceOrg, "Поставщик ресурсов").Column("PUBLIC_SERVORG_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITYOBJECT_ID").NotNull().Fetch();
        }
    }
}
