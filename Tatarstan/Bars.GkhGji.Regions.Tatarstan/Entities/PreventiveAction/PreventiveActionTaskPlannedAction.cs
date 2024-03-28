namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Задание профилактического мероприятия. Запланированное действие
    /// </summary>
    public class PreventiveActionTaskPlannedAction : BaseEntity
    {
        /// <summary>
        /// Задание профилактического мероприятия
        /// </summary>
        public virtual PreventiveActionTask Task { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public virtual string Action { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Commentary { get; set; }
    }
}