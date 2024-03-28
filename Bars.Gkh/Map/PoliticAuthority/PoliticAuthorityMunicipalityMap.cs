/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Органы местного самоуправления мун образований"
///     /// </summary>
///     public class PoliticAuthorityMunicipalityMap : BaseGkhEntityMap<PoliticAuthorityMunicipality>
///     {
///         public PoliticAuthorityMunicipalityMap()
///             : base("GKH_POLITIC_AUTH_MUN")
///         {
///             References(x => x.PoliticAuthority, "POLITIC_AUTH_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Органы государственной власти мун образований"</summary>
    public class PoliticAuthorityMunicipalityMap : BaseImportableEntityMap<PoliticAuthorityMunicipality>
    {
        
        public PoliticAuthorityMunicipalityMap() : 
                base("Органы государственной власти мун образований", "GKH_POLITIC_AUTH_MUN")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.PoliticAuthority, "Орган государственнной власти").Column("POLITIC_AUTH_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
