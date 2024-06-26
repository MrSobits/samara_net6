﻿using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVEmergencyHouse;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРЮЛ через СМЭВ
    /// </summary>
    public interface ISMEVEmergencyHouseService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool SendRequest(SMEVEmergencyHouse requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVEmergencyHouse requestData, SMEV3Library.Entities.GetResponseResponse.GetResponseResponse response, IProgressIndicator indicator = null);
    }
}
