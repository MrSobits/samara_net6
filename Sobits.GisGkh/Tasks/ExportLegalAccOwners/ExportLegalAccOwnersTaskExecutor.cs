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
    /// Задача на массовые запросы получения информации об организациях-абонентах из ГИС ЖКХ
    /// </summary>
    public class ExportLegalAccOwnersTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }

        private IExportOrgRegistryService _ExportOrgRegistryService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public ExportLegalAccOwnersTaskExecutor(IExportOrgRegistryService ExportOrgRegistryService)
        {
            _ExportOrgRegistryService = ExportOrgRegistryService;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var legalContragents = LegalAccountOwnerDomain.GetAll()
                    .Where(x => x.Contragent != null)
                    .Where(x => x.Contragent.GisGkhOrgPPAGUID == null || x.Contragent.GisGkhOrgPPAGUID == "")
                    .Where(x => x.Contragent.Ogrn != null && x.Contragent.Ogrn != "" && x.Contragent.Kpp != null && x.Contragent.Kpp != "")
                    .Select(x => x.Contragent.Id)
                    //.Take(100)
                    .Distinct()
                    .ToList();


                var count = legalContragents.Count();

                var reqNum = count / 100;
                if (count % 100 > 0)
                {
                    reqNum++;
                }
                for (int i = 0; i < reqNum; i++)
                {
                    var legalContragentsPart = legalContragents.Skip(i * 100).Take(100);
                    List<long> contragentIds = new List<long>();
                    foreach (var contragent in legalContragentsPart)
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
