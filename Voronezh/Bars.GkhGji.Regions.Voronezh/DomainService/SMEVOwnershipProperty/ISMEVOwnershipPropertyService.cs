using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Entities.SMEVOwnershipProperty;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРЮЛ через СМЭВ
    /// </summary>
    public interface ISMEVOwnershipPropertyService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool SendRequest(SMEVOwnershipProperty requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVOwnershipProperty requestData, GetResponseResponse response, IProgressIndicator indicator = null);
    }
}
