using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Chelyabinsk.Tasks.MVDSendInformationRequest
{
    public class SendMVDRequestTaskExecutor : ITaskExecutor
    {
        private ISMEVMVDService _SMEVMVDService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<SMEVMVD> SMEVMVDDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public SendMVDRequestTaskExecutor(ISMEVMVDService SMEVMVDService)
        {
            _SMEVMVDService = SMEVMVDService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            SMEVMVD request = SMEVMVDDomain.Get(long.Parse((string)@params.Params["taskId"]));
            if (request == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {
                var result = _SMEVMVDService.SendInformationRequest(request, indicator);
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
