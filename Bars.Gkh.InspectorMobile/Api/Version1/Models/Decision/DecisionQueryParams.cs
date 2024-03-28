namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Decision
{
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums;

    /// <summary>
    /// Параметры запроса для документа "Решение"
    /// </summary>
    public class DecisionQueryParams : BaseDocQueryParams
    {
        /// <summary>
        /// Вид проверки
        /// </summary>
        public DecisionTypeCheck? TypeCheckId { get; set; }
        
        /// <summary>
        /// Контролируемое лицо - физическое лицо
        /// </summary>
        public string Individual { get; set; }
    }
}