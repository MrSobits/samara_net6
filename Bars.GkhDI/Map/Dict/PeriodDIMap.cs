/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Период раскрытия информации"
///     /// </summary>
///     public class PeriodDiMap : BaseGkhEntityMap<PeriodDi>
///     {
///         public PeriodDiMap() : base("DI_DICT_PERIOD")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.DateAccounting, "DATE_ACCOUNTING");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.PeriodDi"</summary>
    public class PeriodDiMap : BaseImportableEntityMap<PeriodDi>
    {
        
        public PeriodDiMap() : 
                base("Bars.GkhDi.Entities.PeriodDi", "DI_DICT_PERIOD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.DateAccounting, "DateAccounting").Column("DATE_ACCOUNTING");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
        }
    }
}
