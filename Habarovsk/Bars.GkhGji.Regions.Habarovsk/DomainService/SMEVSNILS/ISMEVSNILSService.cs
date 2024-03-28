using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    /// <summary>
    /// Сервис работы со СНИЛС
    /// </summary>
    public interface ISMEVSNILSService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool SendInformationRequest(SMEVSNILS requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVSNILS requestData, GetResponseResponse response, IProgressIndicator indicator = null);
    }
}
