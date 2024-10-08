﻿using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Chelyabinsk.Tasks.ERKNMDictRequest
{
    public class SendERKNMDictRequestExecutor : ITaskExecutor
    {
        private IERKNMService _ERKNMService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public SendERKNMDictRequestExecutor(IERKNMService ERKNMService)
        {
            _ERKNMService = ERKNMService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            //отправка
            try
            {
                var dictGUID = @params.Params.GetAs<string>("dictGuid");
                var result = _ERKNMService.SendERKNMDictRequest(dictGUID, indicator);
                if (!result)
                    return new BaseDataResult(false, "Ошибка запроса");
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
