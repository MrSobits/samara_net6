using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Voronezh.Entities.SMEVRedevelopment;
using GetResponseResponse = SMEV3Library.Entities.GetResponseResponse.GetResponseResponse;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРЮЛ через СМЭВ
    /// </summary>
    public interface ISMEVRedevelopmentService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool SendInformationRequest(SMEVRedevelopment requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVRedevelopment requestData, GetResponseResponse response, IProgressIndicator indicator = null);
    }
}
