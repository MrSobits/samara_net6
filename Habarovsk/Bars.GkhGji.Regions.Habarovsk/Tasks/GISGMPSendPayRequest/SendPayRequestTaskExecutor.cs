﻿using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Habarovsk.DomainService;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Habarovsk.Tasks
{
    public class SendPayRequestTaskExecutor : ITaskExecutor
    {
        private IGISGMPService _GISGMPService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGmp> GisGmpDomain { get; set; }

        public IDomainService<GisGmpFile> GisGmpFileDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public SendPayRequestTaskExecutor(IGISGMPService GISGMPService)
        {
            _GISGMPService = GISGMPService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            GisGmp smevRequestData = GisGmpDomain.Get(long.Parse((string)@params.Params["taskId"]));
            if (smevRequestData == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {               
                var result = _GISGMPService.SendPayRequest(smevRequestData, indicator);
                if (!result)
                    return new BaseDataResult(false, smevRequestData.Answer);
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
