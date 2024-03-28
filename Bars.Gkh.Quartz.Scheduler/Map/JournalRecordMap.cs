namespace Bars.Gkh.Quartz.Scheduler.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Quartz.Scheduler.Entities;

    /// <summary>
    /// Маппинг сущности Bars.Gkh.Quartz.Scheduler.Entities.JournalRecord
    /// </summary>
    public class JournalRecordMap : BaseEntityMap<JournalRecord>
    {
        /// <summary>
        /// Конструктор маппинга
        /// </summary>
        public JournalRecordMap()
            : base("Bars.Gkh.Quartz.Scheduler.Entities.JournalRecord", "SCHDLR_JOURNAL")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Trigger, "Trigger").Column("TRIGGER_ID").Fetch();
            this.Property(x => x.StartTime, "StartTime").Column("START_TIME");
            this.Property(x => x.EndTime, "EndTime").Column("END_TIME");
            this.Property(x => x.Result, "Result").Column("RESULT");
            this.Property(x => x.Protocol, "Protocol").Column("PROTOCOL");
            this.Property(x => x.Message, "Message").Column("MESSAGE").Length(10000);
            this.Property(x => x.Interrupted, "Interrupted").Column("INTERRUPTED");
        }
    }
}
