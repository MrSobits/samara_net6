﻿using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVPremises;
using SMEV3Library.Entities.GetResponseResponse;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРЮЛ через СМЭВ
    /// </summary>
    public interface ISMEVPremisesService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool SendRequest(SMEVPremises requestData, IProgressIndicator indicator = null);

        /// <summary>
        /// Обработка ответа
        /// </summary>
        bool TryProcessResponse(SMEVPremises requestData, GetResponseResponse response, IProgressIndicator indicator = null);
    }
}
