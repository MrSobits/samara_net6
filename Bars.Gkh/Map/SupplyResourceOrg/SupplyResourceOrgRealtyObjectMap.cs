/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности  "Связь Поставщика коммунальных услуг с жилым домом"
///     /// </summary>
///     public class SupplyResourceOrgRealtyObjectMap : BaseImportableEntityMap<SupplyResourceOrgRealtyObject>
///     {
///         public SupplyResourceOrgRealtyObjectMap()
///             : base("GKH_SUPPLY_RESORG_RO")
///         {
///             References(x => x.SupplyResourceOrg, "SUPPLY_RESORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITYOBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Связь Поставщика коммунальных услуг с Жилым домом"</summary>
    public class SupplyResourceOrgRealtyObjectMap : BaseImportableEntityMap<SupplyResourceOrgRealtyObject>
    {
        
        public SupplyResourceOrgRealtyObjectMap() : 
                base("Связь Поставщика коммунальных услуг с Жилым домом", "GKH_SUPPLY_RESORG_RO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SupplyResourceOrg, "Поставщик ресурсов").Column("SUPPLY_RESORG_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITYOBJECT_ID").NotNull().Fetch();
        }
    }
}
