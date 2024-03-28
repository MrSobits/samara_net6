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
    /// Формирование запросов на выгрузку начислений в ГИС ЖКХ
    /// </summary>
    public class ImportBillsAccMassTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }

        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }

        private IImportPaymentDocumentDataService _ImportPaymentDocumentDataService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public ImportBillsAccMassTaskExecutor(IImportPaymentDocumentDataService importPaymentDocumentDataService)
        {
            _ImportPaymentDocumentDataService = importPaymentDocumentDataService;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                List<long> lsIds = @params.Params.GetAs<List<long>>("lsIds");
              
                // получаем период
                var period = ChargePeriodDomain.GetAll()
                    .Where(x => x.IsClosed).OrderByDescending(x => x.Id).FirstOrDefault();
                if (period == null)
                {
                    return new BaseDataResult(false, $"Не найден период для выгрузки");
                }
                foreach (long roid in lsIds)
                {
                    var req = new GisGkhRequests
                    {
                        TypeRequest = GisGkhTypeRequest.importPaymentDocumentData,
                        //ReqDate = DateTime.Now,
                        RequestState = GisGkhRequestState.NotFormed
                    };

                    GisGkhRequestsDomain.Save(req);
                    try
                    {
                        _ImportPaymentDocumentDataService.SaveRequest(req, period.Id.ToString(), roid.ToString(), "false");
                    }
                    catch (Exception e)
                    {
                        req.RequestState = GisGkhRequestState.Error;
                        GisGkhRequestsDomain.Update(req);
                        return new BaseDataResult(false, $"{e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
                    }
                }


               

                return new BaseDataResult(true, "Запрос выгрузки ЛС в ГИС ЖКХ созданы");
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
