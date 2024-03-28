using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Sobits.GisGkh.Entities;
using System;

namespace Sobits.GisGkh.DomainService
{
    using Castle.Core;

    /// <summary>
    /// Сервис работы с ГИС ЖКХ
    /// </summary>
    public interface IImportPaymentDocumentDataService
    {
        /// <summary>
        /// Сохранить запрос
        /// </summary>
        /// <param name="req">Запрос</param>
        /// <param name="chargePeriodId">ID периода начислений</param>
        void SaveRequest(GisGkhRequests req, string chargePeriodId, string RoId, string rewrite);

        ///// <summary>
        ///// Сохранить запрос с помощью экзекутора
        ///// </summary>
        ///// <param name="req">Запрос</param>
        ///// <param name="chargePeriodId">ID периода начислений</param>
        //void SaveRequestTaskExecutor(GisGkhRequests req, string chargePeriodId, long? roId, bool rewrite);

        /// <summary>
        /// Сохранить запрос с помощью экзекутора
        /// </summary>
        /// <param name="req">Запрос</param>
        /// <param name="chargePeriodId">ID периода начислений</param>
        void SaveRequestTaskExecutor(GisGkhRequests req, string chargePeriodId, long? roId, bool rewrite, Pair<string,string>[] snaps, bool first);

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

        ///// <summary>
        ///// Запрос квитирования
        ///// </summary>
        ///// <param name="requestData"><see cref=GisGmp/></param>
        ///// <param name="indicator"><see cref=IProgressIndicator/></param>
        ///// <returns>true, если успешно</returns>
        //bool SendReconcileRequest(GisGmp requestData, IProgressIndicator indicator = null);


        ///// <summary>
        ///// Проверяет наличие ответа
        ///// </summary>
        ///// <param name="requestData"><see cref=GisGmp/></param>
        ///// <returns>true, если результат получен</returns>
        //GetResponseResponse CheckResponse(GisGmp requestData);

        ///// <summary>
        ///// Обработка ответа
        ///// </summary>
        ///// <returns>true, если успешно</returns>
        //bool TryProcessResponse(GisGmp requestData, GetResponseResponse response, IProgressIndicator indicator = null);

        //IDataResult GetListLicRequest(BaseParams baseParams);

        //IDataResult GetListReissuance(BaseParams baseParams);

        //IDataResult ListROForGisGkhExport(BaseParams baseParams);
    }
}
