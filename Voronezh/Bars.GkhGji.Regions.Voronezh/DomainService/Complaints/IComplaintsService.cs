using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Entities.PosAppeal;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Voronezh.Entities;
using SMEV3Library.Entities.GetResponseResponse;
// костыль
using System.Xml.Linq;
// костыль - end

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    /// <summary>
    /// Сервис работы с ГИС ГМП через СМЭВ - получение платежей
    /// </summary>
    public interface IComplaintsService
    {


        /// <summary>
        /// Запрос информации о платежах
        /// </summary>
        /// <param name="requestData"><see cref=SMEVComplaintsRequest/></param>
        /// <param name="indicator"><see cref=IProgressIndicator/></param>
        /// <returns>true, если успешно</returns>
        bool SendRequest(SMEVComplaintsRequest requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <returns>true, если успешно</returns>
        bool TryProcessResponse(SMEVComplaintsRequest requestData, GetResponseResponse response, IProgressIndicator indicator = null);

        /// <summary>
        /// Запрос информации о платежах
        /// </summary>
        /// <param name="requestData"><see cref=SMEVComplaintsRequest/></param>
        /// <param name="indicator"><see cref=IProgressIndicator/></param>
        /// <returns>true, если успешно</returns>
        bool CreateAppeals(Rootobject requestData, string token, IProgressIndicator indicator = null);

        IDataResult ListComplaintFiles(BaseParams baseParams);

    }
}
