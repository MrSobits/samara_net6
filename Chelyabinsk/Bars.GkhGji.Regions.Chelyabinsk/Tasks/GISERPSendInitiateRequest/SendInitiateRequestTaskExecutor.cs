using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Chelyabinsk.DomainService;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Chelyabinsk.Tasks
{
    public class SendInitiateRequestTaskExecutor : ITaskExecutor
    {
        private IGISERPService _GISERPService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GISERP> GISERPDomain { get; set; }

        public IDomainService<GISERPFile> GISERPFileDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public SendInitiateRequestTaskExecutor(IGISERPService GISERPService)
        {
            _GISERPService = GISERPService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            GISERP smevRequestData = GISERPDomain.Get(long.Parse((string)@params.Params["taskId"]));
            if(smevRequestData == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {
                var result = _GISERPService.SendInitiationRequest(smevRequestData, indicator);
                if(!result)
                    return new BaseDataResult(false, smevRequestData.Answer);
                else
                    return new BaseDataResult(true, "Сообщение поставлено в очередь СМЭВ");
            }
            catch(HttpRequestException e)
            {
                return new BaseDataResult(false, $"Ошибка связи: {e.InnerException}");
            }
        }
    }
}
