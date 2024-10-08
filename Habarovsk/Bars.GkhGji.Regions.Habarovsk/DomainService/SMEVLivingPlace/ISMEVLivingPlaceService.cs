﻿using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;

//// костыль
//using System.Xml.Linq;
//// костыль - end

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    /// <summary>
    /// Сервис работы с мвд паспорт
    /// </summary>
    public interface ISMEVLivingPlaceService
    {
        /// <summary>
        /// Отправка запроса 
        /// </summary>
        bool SendInformationRequest(SMEVLivingPlace requestData, IProgressIndicator indicator = null);

        bool GetResult(SMEVLivingPlace requestData, IProgressIndicator indicator = null);
    }
}
