using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using Bars.GkhGji.Regions.Voronezh.Entities;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Voronezh.Tasks.SendPaymentRequest
{
    public class SendComplaintsCustomRequestTaskExecutor : ITaskExecutor
    {
        private IComplaintsService _ComplaintsService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<SMEVComplaintsRequest> SMEVComplaintsRequestDomain { get; set; }

        public IDomainService<SMEVComplaintsRequestFile> SMEVComplaintsRequestFileDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public SendComplaintsCustomRequestTaskExecutor(IComplaintsService ComplaintsService)
        {
            _ComplaintsService = ComplaintsService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            SMEVComplaintsRequest smevRequestData = SMEVComplaintsRequestDomain.Get(long.Parse((string)@params.Params["taskId"]));
            if (smevRequestData == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {
                var result = _ComplaintsService.SendRequest(smevRequestData, indicator);
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
