using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Voronezh.Entities;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРЮЛ через СМЭВ
    /// </summary>
    public interface ISMEVChangePremisesStateService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool SendInformationRequest(SMEVChangePremisesState requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVChangePremisesState requestData, GetResponseResponse response, IProgressIndicator indicator = null);
    }
}
