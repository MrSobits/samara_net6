namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using B4;
    using Microsoft.Extensions.Logging;
    using B4.Modules.Tasks.Common.Service;

    using Castle.Windsor;
    using DomainService.PersonalAccount;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Выполняет задачу формирования ПИР
    /// </summary>
    /// <remarks>
    /// Реестр должников / Начать претензионную работу
    /// </remarks>
    public class DebtorClaimWorkTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public string ExecutorCode { get; private set; }

        private readonly IWindsorContainer container;
        private readonly ILogger logger;

        public DebtorClaimWorkTaskExecutor(
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

            var debtorService = this.container.Resolve<IDebtorService>();
            try
            {
                debtorService.CreateClaimWorks(baseParams);
            }
            finally
            {
                this.container.Release(debtorService);
            }

            return new BaseDataResult();
        }
    }
}