using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    /// <summary>
    /// Сервис работы с ГИС ГМП через СМЭВ
    /// </summary>
    public interface IGISGMPService
    {
        /// <summary>
        /// Отправка начисления в ГИС ГМП
        /// </summary>
        /// <param name="requestData"><see cref=GisGmp/></param>
        /// <param name="indicator"><see cref=IProgressIndicator/></param>
        /// <returns>true, если успешно</returns>
        bool SendCalcRequest(GisGmp requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Запрос информации о платежах
        /// </summary>
        /// <param name="requestData"><see cref=GisGmp/></param>
        /// <param name="indicator"><see cref=IProgressIndicator/></param>
        /// <returns>true, если успешно</returns>
        bool SendPayRequest(GisGmp requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Запрос квитирования
        /// </summary>
        /// <param name="requestData"><see cref=GisGmp/></param>
        /// <param name="indicator"><see cref=IProgressIndicator/></param>
        /// <returns>true, если успешно</returns>
        bool SendReconcileRequest(GisGmp requestData, IProgressIndicator indicator = null);


        /// <summary>
        /// Проверяет наличие ответа
        /// </summary>
        /// <param name="requestData"><see cref=GisGmp/></param>
        /// <returns>true, если результат получен</returns>
        GetResponseResponse CheckResponse(GisGmp requestData);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <returns>true, если успешно</returns>
        bool TryProcessResponse(GisGmp requestData, GetResponseResponse response, IProgressIndicator indicator = null);

        IDataResult GetListLicRequest(BaseParams baseParams);

        IDataResult GetListReissuance(BaseParams baseParams);

        IDataResult ListGisGmp(BaseParams baseParams);
    }
}
