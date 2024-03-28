using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.RPGULicRequest;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРЮЛ через СМЭВ
    /// </summary>
    public interface IRPGUService
    {
        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEV3Library.Entities.GetResponseResponse.GetRequestRequest requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool SendAcceptMessage(long requestId, bool sucsess, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool SendLicActionAcceptMessage(long requestId, bool sucsess, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool SendAcceptReissuanceMessage(long requestId, bool sucsess, IProgressIndicator indicator = null);//
    }
}
