﻿using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Chelyabinsk.Tasks
{
    public class MVDLivingPlaceRegistrationRequestTaskExecutor : ITaskExecutor
    {
        private IMVDLivingPlaceRegistrationService _MVDLivingPlaceRegistrationService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<MVDLivingPlaceRegistration> MVDLivingPlaceRegistrationDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public MVDLivingPlaceRegistrationRequestTaskExecutor(IMVDLivingPlaceRegistrationService MVDLivingPlaceRegistrationService)
        {
            _MVDLivingPlaceRegistrationService = MVDLivingPlaceRegistrationService;
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
            MVDLivingPlaceRegistration request = MVDLivingPlaceRegistrationDomain.Get(taskId);
            if (request == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {
                var result = _MVDLivingPlaceRegistrationService.SendInformationRequest(request, indicator);
                if (!result)
                    return new BaseDataResult(false, request.Answer);
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
