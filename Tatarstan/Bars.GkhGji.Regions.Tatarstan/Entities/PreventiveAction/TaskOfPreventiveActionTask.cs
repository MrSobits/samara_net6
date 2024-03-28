namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    /// <summary>
    /// Связь задачи мероприятия с заданием профилактического мероприятия
    /// </summary>
    public class TaskOfPreventiveActionTask : BaseEntity
    {
        /// <summary>
        /// Задание профилактического мероприятия
        /// </summary>
        public virtual PreventiveActionTask PreventiveActionTask { get; set; }
        
        /// <summary>
        /// Задача профилактического мероприятия
        /// </summary>
        public virtual TasksPreventiveMeasures TasksPreventiveMeasures { get; set; }
    }
}