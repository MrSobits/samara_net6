using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Voronezh.Entities;
using SMEV3Library.Entities.GetResponseResponse;

//// костыль
//using System.Xml.Linq;
//// костыль - end

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    /// <summary>
    /// Сервис работы с мвд паспорт
    /// </summary>
    public interface ISMEVValidPassportService
    {
        /// <summary>
        /// Отправка запроса 
        /// </summary>
        bool SendInformationRequest(SMEVValidPassport requestData, IProgressIndicator indicator = null);


    }
}
