using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;
// костыль
using System.Xml.Linq;
// костыль - end

namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    /// <summary>
    /// Сервис работы с ГИС ГМП через СМЭВ - получение платежей
    /// </summary>
    public interface IPAYREGService
    {
        ///// <summary>
        ///// Отправка начисления в ГИС ГМП
        ///// </summary>
        ///// <param name="requestData"><see cref=PayRegRequests/></param>
        ///// <param name="indicator"><see cref=IProgressIndicator/></param>
        ///// <returns>true, если успешно</returns>
        //bool SendCalcRequest(PayRegRequests requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Запрос информации о платежах
        /// </summary>
        /// <param name="requestData"><see cref=PayRegRequests/></param>
        /// <param name="indicator"><see cref=IProgressIndicator/></param>
        /// <returns>true, если успешно</returns>
        bool SendPaymentRequest(PayRegRequests requestData, IProgressIndicator indicator = null);

        ///// <summary>
        ///// Запрос квитирования
        ///// </summary>
        ///// <param name="requestData"><see cref=PayRegRequests/></param>
        ///// <param name="indicator"><see cref=IProgressIndicator/></param>
        ///// <returns>true, если успешно</returns>
        //bool SendReconcileRequest(PayReg requestData, IProgressIndicator indicator = null);


        /// <summary>
        /// Проверяет наличие ответа
        /// </summary>
        /// <param name="requestData"><see cref=PayRegRequests/></param>
        /// <returns>true, если результат получен</returns>
        GetResponseResponse CheckResponse(PayRegRequests requestData);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <returns>true, если успешно</returns>
        bool TryProcessResponse(PayRegRequests requestData, GetResponseResponse response, IProgressIndicator indicator = null);
        //bool TryProcessResponse(PayRegRequests requestData, XElement response, IProgressIndicator indicator = null);

        //IDataResult GetListLicRequest(BaseParams baseParams);

        //IDataResult GetListReissuance(BaseParams baseParams);

        GisGmp FindGisGmp(string UIN, string purpose);

        IDataResult ListPayments(BaseParams baseParams);
        IDataResult ListPaymentsForPayFine(BaseParams baseParams);
        IDataResult AddPayFine(BaseParams baseParams, long resolutionId, long payRegId);
    }
}
