﻿using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using Bars.GkhGji.Regions.Voronezh.Entities;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Voronezh.Tasks
{
    public class MVDStayingPlaceRegistrationRequestTaskExecutor : ITaskExecutor
    {
        private IMVDStayingPlaceRegistrationService _MVDStayingPlaceRegistrationService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<MVDStayingPlaceRegistration> MVDStayingPlaceRegistrationDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public MVDStayingPlaceRegistrationRequestTaskExecutor(IMVDStayingPlaceRegistrationService MVDStayingPlaceRegistrationService)
        {
            _MVDStayingPlaceRegistrationService = MVDStayingPlaceRegistrationService;
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
            MVDStayingPlaceRegistration request = MVDStayingPlaceRegistrationDomain.Get(taskId);
            if (request == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {
                var result = _MVDStayingPlaceRegistrationService.SendInformationRequest(request, indicator);
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
