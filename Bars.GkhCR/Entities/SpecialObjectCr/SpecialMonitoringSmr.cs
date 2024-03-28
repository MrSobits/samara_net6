namespace Bars.GkhCr.Entities
{
    using B4.Modules.States;
    using Gkh.Entities;

    /// <summary>
    /// Мониторинг СМР
    /// </summary>
    public class SpecialMonitoringSmr : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual SpecialObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}
