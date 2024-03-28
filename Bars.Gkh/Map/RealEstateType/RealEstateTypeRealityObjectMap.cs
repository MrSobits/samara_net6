/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.RealEstateType
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.RealEstateType;
/// 
///     public class RealEstateTypeRealityObjectMap : BaseImportableEntityMap<RealEstateTypeRealityObject>
///     {
///         public RealEstateTypeRealityObjectMap()
///             : base("OVRHL_REALESTATEREALITYO")
///         {
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.RealEstateType, "RET_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.RealEstateType
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.RealEstateType;
    
    
    /// <summary>Маппинг для "Связка типа дома - дом"</summary>
    public class RealEstateTypeRealityObjectMap : BaseImportableEntityMap<RealEstateTypeRealityObject>
    {
        
        public RealEstateTypeRealityObjectMap() : 
                base("Связка типа дома - дом", "OVRHL_REALESTATEREALITYO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealEstateType, "Тип дома").Column("RET_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Дом").Column("RO_ID").NotNull().Fetch();
        }
    }
}
