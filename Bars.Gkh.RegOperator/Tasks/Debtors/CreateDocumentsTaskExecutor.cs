namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using B4;
    using Microsoft.Extensions.Logging;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;

    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Выполняет задачу формирования документов ПИР
    /// </summary>
    public class CreateDocumentsTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public string ExecutorCode { get; private set; }

        private readonly IWindsorContainer container;
        private readonly ILogger logger;

        public CreateDocumentsTaskExecutor(
            IWindsorContainer container,
            ILogger logger)
        {
            this.container = container;
            this.logger = logger;
        }

        public IDataResult Execute(BaseParams baseParams,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            
            var clwService = this.container.Resolve<IBaseClaimWorkService<DebtorClaimWork>>();
            try
            {
                indicator.Report(null, 5, "Начато создание документов");
                this.logger.LogInformation("Начато создание документов");

                var documentTypes = baseParams.Params.GetAs("documentTypes", new List<Tuple<ClaimWorkDocumentType, DebtorType>>());
                uint progress = 10;
                uint step = documentTypes.Count > 0? (uint) (90 / documentTypes.Count) : 90;

                foreach (var documentType in documentTypes)
                {
                    var param = new DynamicDictionary();
                    param.SetValue("typeDocument", documentType.Item1);
                    param.SetValue("debtorType", documentType.Item2);

                    indicator.Report(null, progress, $"Создание документов типа '{documentType.Item1.GetDisplayName()}'");

                    clwService.MassCreateDocs(new BaseParams { Params = param });
                    progress += step;
                }
            }
            finally
            {
                this.container.Release(clwService);
            }

            return new BaseDataResult();
        }
    }
}