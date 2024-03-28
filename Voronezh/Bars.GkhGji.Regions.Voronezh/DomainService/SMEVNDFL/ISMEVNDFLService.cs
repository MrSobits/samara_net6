using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Voronezh.Entities;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРЮЛ через СМЭВ
    /// </summary>
    public interface ISMEVNDFLService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool SendInformationRequest(SMEVNDFL requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVNDFL requestData, GetResponseResponse response, IProgressIndicator indicator = null);

        /// <summary>
        /// Получение неподписанной XML
        /// </summary>
        /// <param name="reqId">Id запроса в ГИС ЖКХ</param>
        /// <returns>строка с XML</returns>
        string GetXML(long reqId);
    }
}
