/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RealEstateTypeMunicipalityMap : BaseImportableEntityMap<RealEstateTypeMunicipality>
///     {
///         public RealEstateTypeMunicipalityMap()
///             : base("REAL_EST_TYPE_MU")
///         {
///             References(x => x.RealEstateType, "RET_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Municipality, "MU_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Муниципальное образование типа дома"</summary>
    public class RealEstateTypeMunicipalityMap : BaseImportableEntityMap<RealEstateTypeMunicipality>
    {
        
        public RealEstateTypeMunicipalityMap() : 
                base("Муниципальное образование типа дома", "REAL_EST_TYPE_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealEstateType, "Тип дома").Column("RET_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "Муниципальное образование").Column("MU_ID").NotNull().Fetch();
        }
    }
}
