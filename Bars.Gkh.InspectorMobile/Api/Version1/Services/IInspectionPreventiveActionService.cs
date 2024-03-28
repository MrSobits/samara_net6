using Bars.Gkh.InspectorMobile.Api.Version1.Models.Inspection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    /// <summary>
    /// API-сервис для получения типов документов "Профилактическое мероприятие"
    /// </summary>
    public interface IInspectionPreventiveActionService
    {
        /// <summary>
        /// Получить список документов
        /// </summary>
        Task<IEnumerable<InspectionPreventiveActionTask>> GetListAsync();
    }
}