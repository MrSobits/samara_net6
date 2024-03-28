namespace Bars.GisIntegration.Base.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Quartz.Scheduler.Entities;

    /// <summary>
    /// Связка задач с выполняющими их триггерами
    /// </summary>
    public class RisTaskTrigger : BaseEntity
    {
        /// <summary>
        /// Тип триггера
        /// </summary>
        public virtual TriggerType TriggerType { get; set; }

        /// <summary>
        /// Ссылка на задачу
        /// </summary>
        public virtual RisTask Task { get; set; }

        /// <summary>
        /// Ссылка на триггер
        /// </summary>
        public virtual Trigger Trigger { get; set; }

        /// <summary>
        /// Статус триггера
        /// </summary>
        public virtual TriggerState TriggerState { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public virtual string Message { get; set; }
    }
}