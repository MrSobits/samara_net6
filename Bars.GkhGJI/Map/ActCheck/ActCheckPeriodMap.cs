/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "дата и время проведения проверки акта проверки ГЖИ"
///     /// </summary>
///     public class ActCheckPeriodMap : BaseEntityMap<ActCheckPeriod>
///     {
///         public ActCheckPeriodMap()
///             : base("GJI_ACTCHECK_PERIOD")
///         {
///             Map(x => x.DateCheck, "DATE_CHECK");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
/// 
///             References(x => x.ActCheck, "ACTCHECK_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Дата и время проведения проверки"</summary>
    public class ActCheckPeriodMap : BaseEntityMap<ActCheckPeriod>
    {
        
        public ActCheckPeriodMap() : 
                base("Дата и время проведения проверки", "GJI_ACTCHECK_PERIOD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateCheck, "Дата").Column("DATE_CHECK");
            Property(x => x.DateStart, "Дата начала").Column("DATE_START");
            Property(x => x.DateEnd, "Дата окончания").Column("DATE_END");
            Property(x => x.DurationDays, "Срок проведения (дней)").Column("DURATION_DAYS");
            Property(x => x.DurationHours, "Срок проведения (часов)").Column("DURATION_HOURS");
            Reference(x => x.ActCheck, "Акт проверки").Column("ACTCHECK_ID").NotNull().Fetch();
        }
    }
}
