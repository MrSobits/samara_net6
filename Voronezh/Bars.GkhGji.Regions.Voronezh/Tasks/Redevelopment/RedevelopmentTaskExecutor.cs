using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Entities.SMEVRedevelopment;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Voronezh.Tasks
{
    public class RedevelopmentTaskExecutor : ITaskExecutor
    {
        private ISMEVRedevelopmentService _SMEVRedevelopmentService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<SMEVRedevelopment> SMEVRedevelopmentDomain { get; set; }

        public IDomainService<SMEVRedevelopmentFile> SMEVRedevelopmentFileDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public RedevelopmentTaskExecutor(ISMEVRedevelopmentService SMEVRedevelopmentService)
        {
            _SMEVRedevelopmentService = SMEVRedevelopmentService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            SMEVRedevelopment smevRequestData = SMEVRedevelopmentDomain.Get(long.Parse((string)@params.Params["taskId"]));
            if(smevRequestData == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {
                var result = _SMEVRedevelopmentService.SendInformationRequest(smevRequestData, indicator);
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
