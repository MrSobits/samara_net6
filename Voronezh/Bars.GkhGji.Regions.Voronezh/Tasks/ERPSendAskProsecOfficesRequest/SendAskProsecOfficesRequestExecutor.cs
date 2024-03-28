using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using Bars.GkhGji.Regions.Voronezh.Entities;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Voronezh.Tasks.SendPaymentRequest
{
    public class SendAskProsecOfficesRequestExecutor : ITaskExecutor
    {
        private IGISERPService _GISERPService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;       

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public SendAskProsecOfficesRequestExecutor(IGISERPService GISERPService)
        {
            _GISERPService = GISERPService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
           

            //отправка
            try
            {               
                var result = _GISERPService.SendAskProsecOfficesRequest(indicator);
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
