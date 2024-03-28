using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    /// <summary>
    /// Сервис работы с ГИС ГМП через СМЭВ - получение платежей
    /// </summary>
    public interface IERKNMService
    {

        IDataResult GetListDecision(BaseParams baseParams);

        /// <summary>
        /// Запрос информации о платежах
        /// </summary>
        /// <returns>true, если успешно</returns>
        bool SendAskProsecOfficesRequest(IProgressIndicator indicator = null);

        /// <summary>
        /// Запрос справочника
        /// </summary>
        /// <returns>true, если успешно</returns>
        bool SendERKNMDictRequest(string guid, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <returns>true, если успешно</returns>
        bool TryProcessGetDictResponse(ERKNMDict requestData, GetResponseResponse response, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <returns>true, если успешно</returns>
        bool TryProcessGetProsecutorOfficeResponse(GetResponseResponse response, IProgressIndicator indicator = null);

        object GetInspectionInfo(BaseParams baseParams);

        ERKNM GetERKNMInfo(BaseParams baseParams);

        /// <summary>
        /// Отправка проверки в ГИС ЕРП
        /// </summary>
        /// <param name="requestData"><see cref=GisGmp/></param>
        /// <param name="indicator"><see cref=IProgressIndicator/></param>
        /// <returns>true, если успешно</returns>
        bool SendInitiationRequest(ERKNM requestData, IProgressIndicator indicator = null);


        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <returns>true, если успешно</returns>
        bool TryProcessResponse(ERKNM requestData, GetResponseResponse response, IProgressIndicator indicator = null);

    }
}
