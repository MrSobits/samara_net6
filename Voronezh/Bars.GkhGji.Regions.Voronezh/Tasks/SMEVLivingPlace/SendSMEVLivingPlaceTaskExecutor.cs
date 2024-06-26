﻿using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using Bars.GkhGji.Regions.Voronezh.Entities;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Voronezh.Tasks.SendSMEVLivingPlace
{
    public class SendSMEVLivingPlaceTaskExecutor : ITaskExecutor
    {
        private ISMEVLivingPlaceService _ISMEVLivingPlaceService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<SMEVLivingPlace> SMEVLivingPlaceDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public SendSMEVLivingPlaceTaskExecutor(ISMEVLivingPlaceService SMEVLivingPlaceService)
        {
            _ISMEVLivingPlaceService = SMEVLivingPlaceService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            long taskId = 0;
            try
            {
                taskId = long.Parse(@params.Params["taskId"].ToString());
            }
            catch (Exception e)
            {

            }
            SMEVLivingPlace request = SMEVLivingPlaceDomain.Get(taskId);
            if (request == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {
                var result = _ISMEVLivingPlaceService.SendInformationRequest(request, indicator);
                if (!result)
                    return new BaseDataResult(false, "");
                else
                    return new BaseDataResult(true, "Сообщение поставлено в очередь СМЭВ");
            }
            catch (HttpRequestException e)
            {
                return new BaseDataResult(false, $"Ошибка связи: {e.InnerException}");
            }
        }
    }
}
