/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Органы местного самоуправления мун образований"
///     /// </summary>
///     public class LocalGovernmentMunicipalityMap : BaseGkhEntityMap<LocalGovernmentMunicipality>
///     {
///         public LocalGovernmentMunicipalityMap() : base("GKH_LOCAL_GOV_MUNICIP")
///         {
///             References(x => x.LocalGovernment, "LOCAL_GOV_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Municipality, "MUNICIPALITY_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Органы местного самоуправления мун образований"</summary>
    public class LocalGovernmentMunicipalityMap : BaseImportableEntityMap<LocalGovernmentMunicipality>
    {
        
        public LocalGovernmentMunicipalityMap() : 
                base("Органы местного самоуправления мун образований", "GKH_LOCAL_GOV_MUNICIP")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.LocalGovernment, "Орган местного самоуправления").Column("LOCAL_GOV_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
