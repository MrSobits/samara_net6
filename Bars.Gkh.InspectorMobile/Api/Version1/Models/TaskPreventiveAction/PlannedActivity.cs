namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskPreventiveAction
{
    /// <summary>
    /// Запланированное действие
    /// </summary>
    public class PlannedActivity
    {
        /// <summary>
        /// Действие
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Комментарий к запланированному действию
        /// </summary>
        public string Comment { get; set; }
    }
}