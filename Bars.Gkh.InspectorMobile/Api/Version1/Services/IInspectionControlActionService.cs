namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using System.Collections.Generic;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Inspection;

    /// <summary>
    /// API-сервис для получения типов докуменитов "Задание", "Решение"
    /// </summary>
    public interface IInspectionControlActionService
    {
        /// <summary>
        /// Получить документы "Задание", "Решение"
        /// </summary>
        IEnumerable<InspectionControlAction> GetControlActionDocuments();
    }
}