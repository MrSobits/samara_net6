using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Voronezh.Entities;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    /// <summary>
    /// Сервис работы с МВД через СМЭВ
    /// </summary>
    public interface ISMEVMVDService
    {
        /// <summary>
        /// Отправка запроса на получение справки о судимости
        /// </summary>
        bool SendInformationRequest(SMEVMVD requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVMVD requestData, GetResponseResponse response, IProgressIndicator indicator = null);
     }
}