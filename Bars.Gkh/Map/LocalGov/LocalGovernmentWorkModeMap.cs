/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Режим работы органа местного самоуправления"
///     /// </summary>
///     public class LocalGovernmentWorkModeMap : BaseGkhEntityMap<LocalGovernmentWorkMode>
///     {
///         public LocalGovernmentWorkModeMap() : base("GKH_LOCGOV_WORK")
///         {
///             Map(x => x.TypeMode, "TYPE_MODE").Not.Nullable().CustomType<TypeMode>();
///             Map(x => x.TypeDayOfWeek, "TYPE_DAY").Not.Nullable().CustomType<TypeDayOfWeek>();
///             Map(x => x.StartDate, "START_DATE");
///             Map(x => x.EndDate, "END_DATE");
///             Map(x => x.Pause, "PAUSE").Length(50);
///             Map(x => x.AroundClock, "AROUND_CLOCK").Not.Nullable();
/// 
///             References(x => x.LocalGovernment, "LOCAL_GOVERNMENT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Режим работы органа местного самоуправления"</summary>
    public class LocalGovernmentWorkModeMap : BaseImportableEntityMap<LocalGovernmentWorkMode>
    {
        
        public LocalGovernmentWorkModeMap() : 
                base("Режим работы органа местного самоуправления", "GKH_LOCGOV_WORK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeMode, "Код раздела").Column("TYPE_MODE").NotNull();
            Property(x => x.TypeDayOfWeek, "День недели").Column("TYPE_DAY").NotNull();
            Property(x => x.StartDate, "Время начала").Column("START_DATE");
            Property(x => x.EndDate, "Время окончания").Column("END_DATE");
            Property(x => x.Pause, "Перерыв").Column("PAUSE").Length(50);
            Property(x => x.AroundClock, "Круглоcуточно").Column("AROUND_CLOCK").NotNull();
            Reference(x => x.LocalGovernment, "Орган местного самоуправления").Column("LOCAL_GOVERNMENT_ID").NotNull().Fetch();
        }
    }
}
