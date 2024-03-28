using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;

//// костыль
//using System.Xml.Linq;
//// костыль - end

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    /// <summary>
    /// Сервис работы с Госимущество через СМЭВ 2
    /// </summary>
    public interface ISMEVPropertyTypeService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool SendInformationRequest(SMEVPropertyType requestData, IProgressIndicator indicator = null);


    }
}
