/// <mapping-converter-backup>
/// namespace Bars.Gkh.Reforma.Map.Dict
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Reforma.Entities.Dict;
/// 
///     public class ReportingPeriodDictMap : BaseEntityMap<ReportingPeriodDict>
///     {
///         public ReportingPeriodDictMap()
///             : base("RFRM_REPORTING_PERIOD")
///         {
///             this.Map(x => x.ExternalId, "EXTERNAL_ID", true);
///             this.Map(x => x.Name, "NAME", true);
///             this.Map(x => x.DateStart, "DATE_START", true);
///             this.Map(x => x.DateEnd, "DATE_END", true);
///             this.Map(x => x.State, "STATE", true);
///             this.Map(x => x.Synchronizing, "SYNCHRONIZING", true);
/// 
///             this.References(x => x.PeriodDi, "PERIOD_DI_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Reforma.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Reforma.Entities.Dict;
    
    
    /// <summary>Маппинг для "Периоды раскрытия информации в Реформе"</summary>
    public class ReportingPeriodDictMap : BaseEntityMap<ReportingPeriodDict>
    {
        
        public ReportingPeriodDictMap() : 
                base("Периоды раскрытия информации в Реформе", "RFRM_REPORTING_PERIOD")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PeriodDi, "Период раскрытия в БАРС.ЖКХ").Column("PERIOD_DI_ID");
            Property(x => x.Name, "Название периода").Column("NAME").Length(250).NotNull();
            Property(x => x.DateStart, "Начало периода").Column("DATE_START").NotNull();
            Property(x => x.DateEnd, "Конец периода").Column("DATE_END").NotNull();
            Property(x => x.State, "Состояние периода").Column("STATE").NotNull();
            Property(x => x.ExternalId, "Идентификатор периода в Реформе").Column("EXTERNAL_ID").NotNull();
            Property(x => x.Synchronizing, "Признак синхронизируемости. Синхронизация происходит только по синхронизируемым п" +
                    "ериодам").Column("SYNCHRONIZING").NotNull();
            Property(x => x.Is_988, "Признак отчетного периода. Относится к старым формам или к новым").Column("IS_988").NotNull();
        }
    }
}
