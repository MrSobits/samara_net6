/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Услуга Поставщик коммунальных услуг"
///     /// </summary>
///     public class SupplyResourceOrgServiceMap : BaseGkhEntityMap<SupplyResourceOrgService>
///     {
///         public SupplyResourceOrgServiceMap()
///             : base("GKH_SUPPLY_RESORG_SERV")
///         {
///             References(x => x.SupplyResourceOrg, "SUPPLY_RESORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.TypeService, "TYPE_SERVICE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Услуга Поставщика коммунальных услуг"</summary>
    public class SupplyResourceOrgServiceMap : BaseImportableEntityMap<SupplyResourceOrgService>
    {
        
        public SupplyResourceOrgServiceMap() : 
                base("Услуга Поставщика коммунальных услуг", "GKH_SUPPLY_RESORG_SERV")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.SupplyResourceOrg, "Поставщик коммунальных услуг").Column("SUPPLY_RESORG_ID").NotNull().Fetch();
            Reference(x => x.TypeService, "Тип обслуживания").Column("TYPE_SERVICE_ID").NotNull().Fetch();
        }
    }
}
