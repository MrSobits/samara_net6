/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Ед. измерения"
///     /// </summary>
///     public class UnitMeasureMap : BaseGkhEntityMap<UnitMeasure>
///     {
///         public UnitMeasureMap()
///             : base("GKH_DICT_UNITMEASURE")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.ShortName, "SHORT_NAME").Not.Nullable().Length(20);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;
    
    /// <summary>Маппинг для "Единица измерения"</summary>
    public class UnitMeasureMap : BaseImportableEntityMap<UnitMeasure>
    {
        
        public UnitMeasureMap() : 
                base("Единица измерения", "GKH_DICT_UNITMEASURE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.ShortName, "Короткое наименоваание").Column("SHORT_NAME").Length(20).NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.OkeiCode, "Код ОКЕИ").Column("OKEI_CODE").Length(10);
        }
    }
}
