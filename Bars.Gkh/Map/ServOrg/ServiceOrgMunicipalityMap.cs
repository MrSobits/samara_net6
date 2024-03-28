/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности  "Связь поставщика жил. услуг с МО"
///     /// </summary>
///     public class ServiceOrgMunicipalityMap : BaseImportableEntityMap<ServiceOrgMunicipality>
///     {
///         public ServiceOrgMunicipalityMap()
///             : base("GKH_SERVORG_MU")
///         {
///             References(x => x.ServOrg, "SERVORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Связь поставщика жил. услуг с МО"</summary>
    public class ServiceOrgMunicipalityMap : BaseImportableEntityMap<ServiceOrgMunicipality>
    {
        
        public ServiceOrgMunicipalityMap() : 
                base("Связь поставщика жил. услуг с МО", "GKH_SERVORG_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ServOrg, "Поставщик жил. услуг").Column("SERVORG_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
