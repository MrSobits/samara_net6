/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности  "Связь Поставщика коммунальных услуг с МО"
///     /// </summary>
///     public class SupplyResourceOrgMunicipalityMap : BaseImportableEntityMap<SupplyResourceOrgMunicipality>
///     {
///         public SupplyResourceOrgMunicipalityMap()
///             : base("GKH_SUPPLY_RESORG_MU")
///         {
///             References(x => x.SupplyResourceOrg, "SUPPLY_RESORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Связь Поставщика коммунальных услуг с МО"</summary>
    public class SupplyResourceOrgMunicipalityMap : BaseImportableEntityMap<SupplyResourceOrgMunicipality>
    {
        
        public SupplyResourceOrgMunicipalityMap() : 
                base("Связь Поставщика коммунальных услуг с МО", "GKH_SUPPLY_RESORG_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SupplyResourceOrg, "Поставщик ресурсов").Column("SUPPLY_RESORG_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
