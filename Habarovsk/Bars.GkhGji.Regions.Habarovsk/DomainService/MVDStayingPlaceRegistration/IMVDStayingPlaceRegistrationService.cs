﻿using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;

//// костыль
//using System.Xml.Linq;
//// костыль - end

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРЮЛ через СМЭВ
    /// </summary>
    public interface IMVDStayingPlaceRegistrationService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool SendInformationRequest(MVDStayingPlaceRegistration requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(MVDStayingPlaceRegistration requestData, GetResponseResponse response, IProgressIndicator indicator = null);

        //// костыль
        //void ProcessResponseXML(SMEVEGRIP requestData, XElement data);
        //// костыль - end
    }
}
