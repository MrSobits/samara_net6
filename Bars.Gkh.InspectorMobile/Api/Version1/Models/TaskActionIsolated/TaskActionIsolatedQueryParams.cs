namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskActionIsolated
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Параметры запроса для документа "Задание КНМ"
    /// </summary>
    public class TaskActionIsolatedQueryParams : BaseDocQueryParams
    {
        /// <summary>
        /// Вид проверки
        /// </summary>
        public KindAction? KindAction { get; set; }
        
        /// <summary>
        /// Контролируемое лицо - физическое лицо
        /// </summary>
        public string Individual { get; set; }
    }
}