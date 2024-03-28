/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.RealEstateType
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.RealEstateType;
/// 
///     public class RealEstateTypePriorityParamMap : BaseImportableEntityMap<RealEstateTypePriorityParam>
///     {
///         public RealEstateTypePriorityParamMap()
///             : base("OVRHL_REALESTTYPE_PRIORITY")
///         {
///             Map(x => x.Code, "CODE", true, 300);
///             Map(x => x.Weight, "WEIGHT", true);
///             References(x => x.RealEstateType, "REAL_ESTATE_TYPE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.RealEstateType
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.RealEstateType;
    
    
    /// <summary>Маппинг для "Параметр очередности типа дома"</summary>
    public class RealEstateTypePriorityParamMap : BaseImportableEntityMap<RealEstateTypePriorityParam>
    {
        
        public RealEstateTypePriorityParamMap() : 
                base("Параметр очередности типа дома", "OVRHL_REALESTTYPE_PRIORITY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").Length(300).NotNull();
            Property(x => x.Weight, "Вес").Column("WEIGHT").NotNull();
            Reference(x => x.RealEstateType, "Тип дома").Column("REAL_ESTATE_TYPE_ID").Fetch();
        }
    }
}
