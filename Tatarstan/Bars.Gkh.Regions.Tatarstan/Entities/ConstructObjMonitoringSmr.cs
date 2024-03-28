namespace Bars.Gkh.Regions.Tatarstan.Entities
{
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Мониторинг СМР
    /// </summary>
    public class ConstructObjMonitoringSmr : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual ConstructionObject ConstructionObject { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}
