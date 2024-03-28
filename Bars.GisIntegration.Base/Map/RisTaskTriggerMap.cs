namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;
    using Bars.Gkh.DataAccess;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг сущности Bars.GisIntegration.Base.Entities.RisTaskTrigger
    /// </summary>
    public class RisTaskTriggerMap : BaseEntityMap<RisTaskTrigger>
    {
        /// <summary>
        /// Конструктор маппинга
        /// </summary>
        public RisTaskTriggerMap()
            : base("Bars.GisIntegration.Base.Entities.RisTaskTrigger", "GI_TASK_TRIGGER")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Task, "Task").Column("TASK_ID").Fetch();
            this.Reference(x => x.Trigger, "Trigger").Column("TRIGGER_ID").Fetch();
            this.Property(x => x.TriggerType, "TriggerType").Column("TRIGGER_TYPE");
            this.Property(x => x.TriggerState, "TriggerState").Column("TRIGGER_STATE");
            this.Property(x => x.Message, "Message").Column("MESSAGE");
        }
    }

    public class NhRisTaskTriggerMap : ClassMapping<RisTaskTrigger>
    {
        public NhRisTaskTriggerMap()
        {
            this.Property(
                x => x.Message,
                m =>
                    {
                        m.Type<ImprovedBinaryStringType>();
                    });
        }
    }
}