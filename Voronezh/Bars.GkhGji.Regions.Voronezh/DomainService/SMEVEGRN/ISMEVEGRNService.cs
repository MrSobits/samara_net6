﻿using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Voronezh.Entities;
using SMEV3Library.Entities.GetResponseResponse;

//// костыль
//using System.Xml.Linq;
//// костыль - end

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРН через СМЭВ
    /// </summary>
    public interface ISMEVEGRNService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРН
        /// </summary>
        bool SendInformationRequest(SMEVEGRN requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVEGRN requestData, GetResponseResponse response, IProgressIndicator indicator = null);

        SMEVEGRN GetEGRNInfo(BaseParams baseParams);
        IDataResult GetListRoom(BaseParams baseParams);

        //// костыль
        //void ProcessResponseXML(SMEVEGRIP requestData, XElement data);
        //// костыль - end
    }
}
