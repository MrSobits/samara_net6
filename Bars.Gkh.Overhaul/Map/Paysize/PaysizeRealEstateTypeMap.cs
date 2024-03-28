/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class PaysizeRealEstateTypeMap : BaseImportableEntityMap<PaysizeRealEstateType>
///     {
///         public PaysizeRealEstateTypeMap()
///             : base("OVRHL_PAYSIZE_REC_RET")
///         {
///             Map(x => x.Value, "DVALUE");
/// 
///             References(x => x.RealEstateType, "RET_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Record, "RECORD_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities;
    
    
    /// <summary>Маппинг для "Тип дома размера взноса на кр"</summary>
    public class PaysizeRealEstateTypeMap : BaseImportableEntityMap<PaysizeRealEstateType>
    {
        
        public PaysizeRealEstateTypeMap() : 
                base("Тип дома размера взноса на кр", "OVRHL_PAYSIZE_REC_RET")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Record, "Размер взноса на кр").Column("RECORD_ID").NotNull().Fetch();
            Reference(x => x.RealEstateType, "Тип домов").Column("RET_ID").NotNull().Fetch();
            Property(x => x.Value, "Значение").Column("DVALUE");
        }
    }
}
