/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Периоды"
///     /// </summary>
///     public class PeriodMap : BaseGkhEntityMap<Period>
///     {
///         public PeriodMap()
///             : base("GKH_DICT_PERIOD")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.DateStart, "DATE_START").Not.Nullable();
///             Map(x => x.DateEnd, "DATE_END");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Период"</summary>
    public class PeriodMap : BaseImportableEntityMap<Period>
    {
        
        public PeriodMap() : 
                base("Период", "GKH_DICT_PERIOD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.DateStart, "Дата начала").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
        }
    }
}
