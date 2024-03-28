namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskPreventiveAction
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Параметры запроса для документа "Задание профилактического мероприятия"
    /// </summary>
    public class TaskPreventiveActionQueryParams : BaseDocQueryParams
    {
        /// <summary>
        /// Вид мероприятия
        /// </summary>
        public PreventiveActionType? TypeCheckId { get; set; }
        
        /// <summary>
        /// Тип визита
        /// </summary>
        public PreventiveActionVisitType? TypeVisit { get; set; }
    }
}