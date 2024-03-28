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
    /// Задача на массовые запросы получения информации об организациях из ГИС ЖКХ
    /// </summary>
    public class ExportOrgTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        private IExportOrgRegistryService _ExportOrgRegistryService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public ExportOrgTaskExecutor(IExportOrgRegistryService ExportOrgRegistryService)
        {
            _ExportOrgRegistryService = ExportOrgRegistryService;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var contragents = ContragentDomain.GetAll()
                    .Where(x => x.GisGkhVersionGUID == null || x.GisGkhVersionGUID == "")
                    .Where(x => x.Ogrn != null && x.Ogrn != "" && x.Ogrn.Trim().Length == 13
                        && x.Kpp != null && x.Kpp != "" && x.Kpp.Trim().Length == 9)
                    .Select(x => x.Id)
                    //.Take(100)
                    .ToList();


                var count = contragents.Count();

                var reqNum = count / 100;
                if (count % 100 > 0)
                {
                    reqNum++;
                }
                for (int i = 0; i < reqNum; i++)
                {
                    var contragentsPart = contragents.Skip(i * 100).Take(100);
                    List<long> contragentIds = new List<long>();
                    foreach (var contragent in contragentsPart)
                    {
                        contragentIds.Add(contragent);
                    }
                    var req = new GisGkhRequests
                    {
                        TypeRequest = GisGkhTypeRequest.exportOrgRegistry,
                        //ReqDate = DateTime.Now,
                        RequestState = GisGkhRequestState.NotFormed
                    };
                    GisGkhRequestsDomain.Save(req);
                    try
                    {
                        _ExportOrgRegistryService.SaveRequest(req, contragentIds);
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
