using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Sobits.GisGkh.DomainService;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Sobits.GisGkh.Tasks.ProcessGisGkhAnswers
{
    /// <summary>
    /// Задача на массовые запросы получения решений о формировании ФКР из ГИС ЖКХ
    /// </summary>
    public class ExportDecisionsFormingFundTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        private IExportDecisionsFormingFundService _ExportDecisionsFormingFundService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public ExportDecisionsFormingFundTaskExecutor(IExportDecisionsFormingFundService ExportDecisionsFormingFundService)
        {
            _ExportDecisionsFormingFundService = ExportDecisionsFormingFundService;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var RObjects = RealityObjectDomain.GetAll()
                    .Select(x => x.Id);
                var count = RObjects.Count();

                var reqNum = count / 1000;
                if (count % 1000 > 0)
                {
                    reqNum++;
                }
                for (int i = 0; i < reqNum; i++)
                {
                    var RObjectsPart = RObjects.Skip(i * 1000).Take(1000);
                    List<long> roIds = new List<long>();
                    foreach (var RO in RObjectsPart)
                    {
                        roIds.Add(RO);
                    }
                    var req = new GisGkhRequests
                    {
                        TypeRequest = GisGkhTypeRequest.exportDecisionsFormingFund,
                        //ReqDate = DateTime.Now,
                        RequestState = GisGkhRequestState.NotFormed
                    };
                    GisGkhRequestsDomain.Save(req);
                    try
                    {
                        _ExportDecisionsFormingFundService.SaveRequest(req, roIds);
                    }
                    catch (Exception e)
                    {
                        req.RequestState = GisGkhRequestState.Error;
                        req.Answer = $"Ошибка при сохранении запроса: {e.GetType()} {e.Message}";
                        GisGkhRequestsDomain.Update(req);
                        throw e;
                    }
                }
                return new BaseDataResult(true, "Запросы созданы");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, $"{e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
            }
        }

        #endregion

        #region Private methods

        #endregion
    }
}
