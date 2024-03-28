using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Entities.SMEVPremises;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Voronezh.Tasks
{
    public class PremisesTaskExecutor : ITaskExecutor
    {
        private ISMEVPremisesService _SMEVPremisesService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<SMEVPremises> SMEVPremisesDomain { get; set; }

        public IDomainService<SMEVPremisesFile> SMEVPremisesFileDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public PremisesTaskExecutor(ISMEVPremisesService SMEVPremisesService)
        {
            _SMEVPremisesService = SMEVPremisesService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            SMEVPremises smevRequestData = SMEVPremisesDomain.Get(long.Parse((string)@params.Params["taskId"]));
            if(smevRequestData == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {
                var result = _SMEVPremisesService.SendRequest(smevRequestData, indicator);
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
