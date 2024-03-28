namespace Bars.Gkh.Quartz.Scheduler.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Quartz.Scheduler.Entities;

    /// <summary>
    /// Маппинг сущности Bars.Gkh.Quartz.Scheduler.Entities.Trigger
    /// </summary>
    public class TriggerMap : BaseEntityMap<Trigger>
    {
        /// <summary>
        /// Конструктор маппинга 
        /// </summary>
        public TriggerMap()
            : base("Bars.Gkh.Quartz.Scheduler.Entities.Trigger", "SCHDLR_TRIGGER")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ClassName, "ClassName").Column("CLASS_NAME").Length(200);
            this.Property(x => x.QuartzTriggerKey, "QuartzTriggerKey").Column("QRTZ_TRIGGER_KEY").Length(200);
            this.Property(x => x.StartParams, "StartParams").Column("START_PARAMS");
            this.Property(x => x.RepeatCount, "RepeatCount").Column("REPEAT_COUNT");
            this.Property(x => x.Interval, "Interval").Column("INTERVAL");
            this.Property(x => x.StartTime, "StartTime").Column("START_TIME");
            this.Property(x => x.EndTime, "EndTime").Column("END_TIME");
            this.Property(x => x.UserName, "UserName").Column("USER_NAME").Length(200);
        }
    }
}
