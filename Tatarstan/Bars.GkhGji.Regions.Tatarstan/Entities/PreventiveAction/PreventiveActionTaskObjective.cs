namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    /// <summary>
    /// Связь между заданием и целью профилактического мероприятия
    /// </summary>
    public class PreventiveActionTaskObjective : BaseEntity
    {
        /// <summary>
        /// Задание профилактического мероприятия
        /// </summary>
        public virtual PreventiveActionTask PreventiveActionTask { get; set; }

        /// <summary>
        /// Цель профилактического мероприятия
        /// </summary>
        public virtual ObjectivesPreventiveMeasure ObjectivesPreventiveMeasure { get; set; }
    }
}