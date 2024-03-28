/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.RealEstateType
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.RealEstateType;
/// 
///     public class RealEstateTypeCommonParamMap : BaseImportableEntityMap<RealEstateTypeCommonParam>
///     {
///         public RealEstateTypeCommonParamMap() : base("REAL_EST_TYPE_COMM_PARAM") 
///         {
///             Map(x => x.Min, "MIN", false, 500);
///             Map(x => x.Max, "MAX", false, 500);
///             Map(x => x.CommonParamCode, "COMMON_PARAM_CODE", false, 500);
/// 
///             References(x => x.RealEstateType, "REAL_EST_TYPE_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.RealEstateType
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.RealEstateType;
    
    
    /// <summary>Маппинг для "Общий параметр типа жилых домов"</summary>
    public class RealEstateTypeCommonParamMap : BaseImportableEntityMap<RealEstateTypeCommonParam>
    {
        
        public RealEstateTypeCommonParamMap() : 
                base("Общий параметр типа жилых домов", "REAL_EST_TYPE_COMM_PARAM")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealEstateType, "Тип домов").Column("REAL_EST_TYPE_ID").NotNull().Fetch();
            Property(x => x.CommonParamCode, "Код общего параметра").Column("COMMON_PARAM_CODE").Length(500);
            Property(x => x.Min, "Минимальное значение").Column("MIN").Length(500);
            Property(x => x.Max, "Максимальное значение").Column("MAX").Length(500);
        }
    }
}
