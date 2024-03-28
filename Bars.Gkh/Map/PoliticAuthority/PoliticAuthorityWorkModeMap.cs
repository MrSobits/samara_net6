/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Режим работы органа местного самоуправления"
///     /// </summary>
///     public class PoliticAuthorityWorkModeMap : BaseGkhEntityMap<PoliticAuthorityWorkMode>
///     {
///         public PoliticAuthorityWorkModeMap()
///             : base("GKH_POLITIC_AUTH_WORK")
///         {
///             Map(x => x.TypeMode, "TYPE_MODE").Not.Nullable().CustomType<TypeMode>();
///             Map(x => x.TypeDayOfWeek, "TYPE_DAY").Not.Nullable().CustomType<TypeDayOfWeek>();
///             Map(x => x.StartDate, "START_DATE");
///             Map(x => x.EndDate, "END_DATE");
///             Map(x => x.Pause, "PAUSE").Length(50);
///             Map(x => x.AroundClock, "AROUND_CLOCK").Not.Nullable();
/// 
///             References(x => x.PoliticAuthority, "POLITIC_AUTH_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Режим работы органа государственной власти местного самоуправления"</summary>
    public class PoliticAuthorityWorkModeMap : BaseImportableEntityMap<PoliticAuthorityWorkMode>
    {
        
        public PoliticAuthorityWorkModeMap() : 
                base("Режим работы органа государственной власти местного самоуправления", "GKH_POLITIC_AUTH_WORK")
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
            Reference(x => x.PoliticAuthority, "Орган государственной власти").Column("POLITIC_AUTH_ID").NotNull().Fetch();
        }
    }
}
