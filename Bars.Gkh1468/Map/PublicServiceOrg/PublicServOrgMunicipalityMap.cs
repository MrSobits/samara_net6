/// <mapping-converter-backup>
/// namespace Bars.Gkh1468.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh1468.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности  "Связь Поставщика коммунальных услуг с МО"
///     /// </summary>
///     public class PublicServOrgMunicipalityMap : BaseEntityMap<PublicServiceOrgMunicipality>
///     {
///         public PublicServOrgMunicipalityMap()
///             : base("GKH_PUBLIC_SERVORG_MU")
///         {
///             References(x => x.PublicServiceOrg, "PUBLIC_SERVORG_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh1468.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh1468.Entities;
    
    
    /// <summary>Маппинг для "Связь Поставщик коммунальных услуг с МО"</summary>
    public class PublicServiceOrgMunicipalityMap : BaseEntityMap<PublicServiceOrgMunicipality>
    {
        
        public PublicServiceOrgMunicipalityMap() : 
                base("Связь Поставщик коммунальных услуг с МО", "GKH_PUBLIC_SERVORG_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PublicServiceOrg, "Поставщик ком. услуг").Column("PUBLIC_SERVORG_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
