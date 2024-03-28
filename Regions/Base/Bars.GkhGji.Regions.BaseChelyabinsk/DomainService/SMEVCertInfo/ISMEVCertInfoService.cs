using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРН через СМЭВ
    /// </summary>
    public interface ISMEVCertInfoService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРН
        /// </summary>
        bool SendInformationRequest(SMEVCertInfo requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVCertInfo requestData, GetResponseResponse response, IProgressIndicator indicator = null);
    }
}
