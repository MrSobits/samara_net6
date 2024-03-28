using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.RegOperator.Entities;
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
    /// Задача на массовые запросы получения информации о домах из ГИС ЖКХ
    /// </summary>
    public class ExportAccDataTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        private IExportAccountDataService _ExportAccountDataService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public ExportAccDataTaskExecutor(IExportAccountDataService ExportAccountDataService)
        {
            _ExportAccountDataService = ExportAccountDataService;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                //var rowithBPAfromGis = BasePersonalAccountDomain.GetAll()
                //    .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "")
                //    .Select(x => x.Room.RealityObject.Id).Distinct().ToList();

                //var RObjects = RealityObjectDomain.GetAll()
                //    .Where(x=> !rowithBPAfromGis.Contains(x.Id))
                //    .Where(x => x.GisGkhMatchDate != null)
                //    .OrderBy(x => x.GisGkhAccMatchDate)
                //    .Take(500)
                //    .ToList();
                var RObjects = RealityObjectDomain.GetAll()
                   .Where(x => x.ImportEntityId == 1)
                   .ToList();
                foreach (var RO in RObjects)
                {
                    var req = new GisGkhRequests
                    {
                        TypeRequest = GisGkhTypeRequest.exportAccountData,
                        //ReqDate = DateTime.Now,
                        RequestState = GisGkhRequestState.NotFormed
                    };
                    
                    GisGkhRequestsDomain.Save(req);
                    try
                    {
                        _ExportAccountDataService.SaveRequest(req, RO.Id.ToString());
                    }
                    catch (Exception e)
                    {
                        req.RequestState = GisGkhRequestState.Error;
                        GisGkhRequestsDomain.Update(req);
                        return new BaseDataResult(false, $"{e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
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
