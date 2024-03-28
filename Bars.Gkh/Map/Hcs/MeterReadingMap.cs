/// <mapping-converter-backup>
/// using Bars.B4.DataAccess.ByCode;
/// using Bars.Gkh.Entities.Hcs;
/// namespace Bars.Gkh.Map.Hcs
/// {
///     public class MeterReadingMap : BaseImportableEntityMap<MeterReading>
///     {
///         public MeterReadingMap()
///             : base("HCS_METER_READING")
///         {
///             Map(x => x.Service, "SERVICE");
///             Map(x => x.MeterSerial, "METER_SERIAL");
///             Map(x => x.MeterType, "METER_TYPE");
///             Map(x => x.CurrentReadingDate, "CURRENT_READ_DATE");
///             Map(x => x.PrevReadingDate, "PREV_READ_DATE");
///             Map(x => x.CurrentReading, "CURRENTE_READ");
///             Map(x => x.PrevReading, "PREV_READ");
///             Map(x => x.Expense, "EXPENSE");
///             Map(x => x.PlannedExpense, "PLANNED_EXPENSE");
///             Map(x => x.CompositeKey, "COMPOSITE_KEY", false, 100);
/// 
///             References(x => x.Account, "ACCOUNT_ID", ReferenceMapConfig.Fetch);
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Hcs
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Hcs;
    
    
    /// <summary>Маппинг для "Показания приборов учета (для лицевого счета)"</summary>
    public class MeterReadingMap : BaseImportableEntityMap<MeterReading>
    {
        
        public MeterReadingMap() : 
                base("Показания приборов учета (для лицевого счета)", "HCS_METER_READING")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.Account, "Лицевой счет дома").Column("ACCOUNT_ID").Fetch();
            Property(x => x.Service, "Услуга").Column("SERVICE").Length(250);
            Property(x => x.MeterSerial, "Номер Прибора учета").Column("METER_SERIAL").Length(250);
            Property(x => x.MeterType, "Тип Прибора учета").Column("METER_TYPE").Length(250);
            Property(x => x.CurrentReadingDate, "Текущая дата снятия").Column("CURRENT_READ_DATE");
            Property(x => x.PrevReadingDate, "Предыдущая дата снятия").Column("PREV_READ_DATE");
            Property(x => x.CurrentReading, "Текущие показания").Column("CURRENTE_READ");
            Property(x => x.PrevReading, "Предыдущие показания").Column("PREV_READ");
            Property(x => x.Expense, "Расход").Column("EXPENSE");
            Property(x => x.PlannedExpense, "Плановый расход").Column("PLANNED_EXPENSE");
            Property(x => x.CompositeKey, "Составной ключ вида Account.PaymentCode#MeterSerial#Service, формируется в интерц" +
                    "епторе на Create и Update Не отображать в клиентской части").Column("COMPOSITE_KEY").Length(100);
        }
    }
}
