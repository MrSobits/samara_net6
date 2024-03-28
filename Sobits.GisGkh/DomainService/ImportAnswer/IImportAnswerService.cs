using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Sobits.GisGkh.Entities;

namespace Sobits.GisGkh.DomainService
{
    /// <summary>
    /// Сервис работы с ГИС ЖКХ
    /// </summary>
    public interface IImportAnswerService
    {
        /// <summary>
        /// Сохранить запрос на назначение инспектора обращению
        /// </summary>
        /// <param name="req">Запрос</param>
        /// <param name="appeal">Обращение</param>
        /// <param name="inspector">Инспектор</param>
        void SaveAppealRequest(GisGkhRequests req, AppealCits appeal, Inspector inspector);

        /// <summary>
        /// Сохранить запрос на закрытие обращения
        /// </summary>
        /// <param name="req">Запрос</param>
        /// <param name="appeal">Обращение</param>
        /// <param name="closeType">Тип закрытия</param>
        /// <param name="OrgPPAGUID">Идентификатор организации</param>
        void SaveAnswerRequest(GisGkhRequests req, AppealCits appeal, AppealAnswerType closeType, string OrgPPAGUID);

        /// <summary>
        /// Сохранить запрос на продление рассмотрения обращения
        /// </summary>
        /// <param name="req">Запрос</param>
        /// <param name="appeal">Обращение</param>
        /// <param name="OrgPPAGUID">Идентификатор организации</param>
        void SaveRollOverRequest(GisGkhRequests req, AppealCits appeal, string OrgPPAGUID);

        /// <summary>
        /// Сохранить запрос на переадресацию обращения
        /// </summary>
        /// <param name="req">Запрос</param>
        /// <param name="appeal">Обращение</param>
        /// <param name="OrgPPAGUID">Идентификатор организации</param>
        void SaveRedirectRequest(GisGkhRequests req, AppealCits appeal, string OrgPPAGUID);

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
