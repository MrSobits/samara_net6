using Bars.B4;
using Bars.B4.Config;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.ConfigSections.GisGkh;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Castle.Windsor;
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
    public class ExportROTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        private IExportHouseDataService _ExportHouseDataService;

        private IWindsorContainer _container;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public ExportROTaskExecutor(IExportHouseDataService ExportHouseDataService, IWindsorContainer container)
        {
            _ExportHouseDataService = ExportHouseDataService;
            _container = container;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var numberHouseMassExport =
                        this._container.GetGkhConfig<GisGkhConfig>().numberHouseMassExport;
                var RObjects = RealityObjectDomain.GetAll()
                   .Where(x => x.HouseGuid != null && x.HouseGuid != "" && x.FiasAddress.HouseGuid.HasValue && x.Municipality != null)
                   .Where(x => x.TypeHouse == TypeHouse.ManyApartments || x.TypeHouse == TypeHouse.SocialBehavior)
                   .Where(x => x.ConditionHouse == ConditionHouse.Serviceable)
                   .Where(x => x.State.Name != "Скорректирован" && (x.GisGkhGUID == null || x.GisGkhGUID == ""))
                   .OrderBy(x => x.GisGkhMatchDate ?? DateTime.MinValue)
                   .Take(numberHouseMassExport)
                   .ToList();
                foreach (var RO in RObjects)
                {
                    var req = new GisGkhRequests
                    {
                        TypeRequest = GisGkhTypeRequest.exportHouseData,
                        //ReqDate = DateTime.Now,
                        RequestState = GisGkhRequestState.NotFormed
                    };
                    
                    GisGkhRequestsDomain.Save(req);
                    try
                    {
                        _ExportHouseDataService.SaveRequest(req, RO.Id.ToString());
                        RO.GisGkhMatchDate = DateTime.Now;
                        RealityObjectDomain.Update(RO);
                    }
                    catch (Exception e)
                    {
                        req.RequestState = GisGkhRequestState.Error;
                        GisGkhRequestsDomain.Update(req);
                        return new BaseDataResult(false, $"{e.GetType()} {RO.Municipality.Name}, {RO.Address} {e.Message} {e.InnerException} {e.StackTrace}");
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
