using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    /// <summary>
    /// Сервис работы с ЕРУЛ через СМЭВ
    /// </summary>
    public interface ISMEVERULService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool SendInformationRequest(SMEVERULReqNumber requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVERULReqNumber requestData, GetResponseResponse response, IProgressIndicator indicator = null);
    }
}
