namespace Bars.GkhCr.Entities
{
    using B4.Modules.States;
    using Gkh.Entities;

    /// <summary>
    /// Мониторинг СМР
    /// </summary>
    public class MonitoringSmr : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}
