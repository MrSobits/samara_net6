/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности  "Связь поставщика управляющей организации с МО"
///     /// </summary>
///     public class ManagingOrgMunicipalityMap : BaseImportableEntityMap<ManagingOrgMunicipality>
///     {
///         public ManagingOrgMunicipalityMap()
///             : base("GKH_MANORG_MU")
///         {
///             References(x => x.ManOrg, "MANORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Связь управляющей организации с МО"</summary>
    public class ManagingOrgMunicipalityMap : BaseImportableEntityMap<ManagingOrgMunicipality>
    {
        
        public ManagingOrgMunicipalityMap() : 
                base("Связь управляющей организации с МО", "GKH_MANORG_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ManOrg, "Управляющая организация").Column("MANORG_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
