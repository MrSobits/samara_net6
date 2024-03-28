namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Modules.Tasks.Common.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Impl;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Executor для обновления статусов неплательщиков
    /// </summary>
    public class DebtorsStateTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private const int PortionSize = 2500;

        /// <inheritdoc />
        public string ExecutorCode { get; private set; }

        private readonly IWindsorContainer container;
        private readonly ILogger logger;

        public IDebtorClaimWorkUpdateService UpdateService { get; set; }

        /// <inheritdoc />
        public DebtorsStateTaskExecutor(
            IWindsorContainer container,
            ILogger logger)
        {
            this.container = container;
            this.logger = logger;
        } 

        /// <inheritdoc />
        public IDataResult Execute(BaseParams baseParams,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var debtorClaimWorkDomain = this.container.ResolveDomain<DebtorClaimWork>();

            var sessionProvider = this.container.Resolve<ISessionProvider>();

            var debtorType = baseParams.Params.GetAs("debtorType", DebtorType.NotSet);
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

            try
            {
                var debtorQuery = debtorClaimWorkDomain.GetAll()
                    .WhereIf(debtorType != DebtorType.NotSet, x => x.DebtorType == debtorType);

                this.UpdateService.SetDefaultState(debtorQuery.Where(x => x.State == null));

                var debtorClaimWorkQuery = debtorQuery
                    .WhereIf(ids.Length > 0, x => ids.Contains(x.Id))
                    .Where(x => !x.State.FinalState);

                var count = debtorClaimWorkQuery.Count();
                var take = DebtorsStateTaskExecutor.PortionSize;
                for (int skip = 0; skip < count; skip += take)
                {
                    var debtorPortion = debtorClaimWorkQuery
                        .Skip(skip)
                        .Take(take)
                        .Select(x => x.Id)
                        .ToArray();

                    this.UpdateService.UpdateStates(debtorPortion);

                    int executed = Math.Min(skip + take, count);

                    indicator.Indicate(
                        null,
                        (ushort)((float)executed * 100 / count),
                        $"Обработано {executed} из {count} неплательщиков");

                    this.logger.LogInformation($"(Обновление неплательщиков) Обработано {executed} из {count} ЛС");

                    sessionProvider.GetCurrentSession().Clear();
                }
            }
            finally
            {
                this.container.Release(debtorClaimWorkDomain);
            }

            return new BaseDataResult();
        }
    }
}
