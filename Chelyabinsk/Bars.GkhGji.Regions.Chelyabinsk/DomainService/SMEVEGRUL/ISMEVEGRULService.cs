using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРЮЛ через СМЭВ
    /// </summary>
    public interface ISMEVEGRULService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool SendInformationRequest(SMEVEGRUL requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVEGRUL requestData, GetResponseResponse response, IProgressIndicator indicator = null);
    }
}
