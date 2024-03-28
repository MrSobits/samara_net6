using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Sobits.GisGkh.Entities;
using System;

namespace Sobits.GisGkh.DomainService
{
    /// <summary>
    /// Сервис работы с ГИС ЖКХ
    /// </summary>
    public interface IExportAppealService
    {
        /// <summary>
        /// Сохранить запрос
        /// </summary>
        /// <param name="req">Запрос</param>
        /// <param name="start">Начальная дата</param>
        /// <param name="end">Конечная дата</param>
        void SaveRequest(GisGkhRequests req, DateTime? start, DateTime? end);

        /// <summary>
        /// Проверить ответы
        /// </summary>
        /// <param name="req">Запрос в ГИС ЖКХ</param>
        /// <param name="orgPPAGUID">GUID организации</param>
        void CheckAnswer(GisGkhRequests req, string orgPPAGUID);

        /// <summary>
        /// Обработать ответ
        /// </summary>
        /// <param name="req">Запрос в ГИС ЖКХ</param>
        void ProcessAnswer(GisGkhRequests req);
    }
}
