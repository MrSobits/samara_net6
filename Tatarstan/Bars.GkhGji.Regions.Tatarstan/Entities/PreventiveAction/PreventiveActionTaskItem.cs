namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    /// <summary>
    /// Предмет задания профилактического мероприятия
    /// </summary>
    public class PreventiveActionTaskItem : BaseEntity
    {
        /// <summary>
        /// Задание профилактического мероприятия
        /// </summary>
        public virtual PreventiveActionTask Task { get; set; }

        /// <summary>
        /// Предмет профилактического мероприятия
        /// </summary>
        public virtual PreventiveActionItems Item { get; set; }
    }
}