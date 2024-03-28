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
    using Castle.Core;

    /// <summary>
    /// Формирование запросов на выгрузку начислений в ГИС ЖКХ
    /// </summary>
    public class ImportPaymentDocumentsTaskExecutor : ITaskExecutor
    {
        #region Properties

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }

        private IImportPaymentDocumentDataService _ImportPaymentDocumentDataService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        #endregion

        #region Constructors

        public ImportPaymentDocumentsTaskExecutor(IImportPaymentDocumentDataService ImportPaymentDocumentDataService)
        {
            _ImportPaymentDocumentDataService = ImportPaymentDocumentDataService;
        }

        #endregion

        #region Public methods

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var req = GisGkhRequestsDomain.Get(@params.Params.GetValue("reqId"));
                var chargePeriodId = @params.Params.GetValue("chargePeriodId");
                var roId = @params.Params.GetValue("roId");
                var rewrite = @params.Params.GetValue("rewrite");
                var snaps = @params.Params.GetValue("snaps");
                var first = @params.Params.GetValue("first");

                _ImportPaymentDocumentDataService.SaveRequestTaskExecutor(req, (string)chargePeriodId, (long?)roId, (bool)rewrite, (Pair<string,string>[])snaps, (bool)first);
                   
                return new BaseDataResult(true, "Запросы выгрузки начислений в ГИС ЖКХ созданы");
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
