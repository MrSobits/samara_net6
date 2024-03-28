/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class VersionRecordStage2Map : BaseImportableEntityMap<VersionRecordStage2>
///     {
///         public VersionRecordStage2Map()
///             : base("OVRHL_STAGE2_VERSION")
///         {
///             Map(x => x.CommonEstateObjectWeight, "CE_WEIGHT", true, 0);
///             Map(x => x.Sum, "SUM", true, 0);
/// 
///             References(x => x.Stage3Version, "ST3_VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.CommonEstateObject, "COMMON_ESTATE_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Версионирование второго этапа"</summary>
    public class VersionRecordStage2Map : BaseImportableEntityMap<VersionRecordStage2>
    {
        
        public VersionRecordStage2Map() : 
                base("Версионирование второго этапа", "OVRHL_STAGE2_VERSION")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Stage3Version, "Версия 3го этапа").Column("ST3_VERSION_ID").NotNull().Fetch();
            Property(x => x.CommonEstateObjectWeight, "Вес конструктивного элемента").Column("CE_WEIGHT").NotNull();
            Property(x => x.Sum, "Сумма по 2му этапу").Column("SUM").NotNull();
            Reference(x => x.CommonEstateObject, "Объект общего имущества").Column("COMMON_ESTATE_ID").NotNull().Fetch();
        }
    }
}
