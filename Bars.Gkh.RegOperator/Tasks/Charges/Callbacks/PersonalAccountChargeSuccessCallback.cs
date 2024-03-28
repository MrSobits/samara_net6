namespace Bars.Gkh.RegOperator.Tasks.Charges.Callbacks
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils.PerformanceLogging;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Хендлер успешного завершения расчётов
    /// </summary>
    public class PersonalAccountChargeSuccessCallback : ITaskCallback
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;
        private readonly ISessionProvider sessionProvider;
        private readonly IPerformanceLoggerFactory loggerFactory;
        private readonly IPersonalAccountService personalAccountService;
        private readonly IPersonalAccountFilterService filterService;
        private readonly IChargePeriodRepository periodRepository;
        private readonly IDomainService<BasePersonalAccount> personalAccountDomain;

        /// <summary>
        /// .ctor
        /// </summary>
        public PersonalAccountChargeSuccessCallback(
            IWindsorContainer container, 
            ISessionProvider sessionProvider, 
            IPerformanceLoggerFactory loggerFactory,
            IPersonalAccountService personalAccountService,
            IPersonalAccountFilterService filterService,
            IChargePeriodRepository periodRepository,
            IDomainService<BasePersonalAccount> personalAccountDomain)
        {
            this.container = container;
            this.sessionProvider = sessionProvider;
            this.loggerFactory = loggerFactory;
            this.filterService = filterService;
            this.periodRepository = periodRepository;
            this.personalAccountDomain = personalAccountDomain;
            this.personalAccountService = personalAccountService;
        }

        /// <summary>
        /// Запустить хендлер
        /// </summary>
        public CallbackResult Call(
            long taskId,
            BaseParams baseParams,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var cfg = baseParams.Params.GetAs<OperationLockConfig>("operationLock");
            if (cfg != null && cfg.Enabled && !cfg.PreserveLockAfterCalc)
            {
                PersonalAccountChargeTableLocker.Unlock(this.container);
            }

            var calcGuid = baseParams.Params.GetAs<string>("calcGuid");
            var packetId = baseParams.Params.GetAs<long>("packetId", ignoreCase: true);

            var logger = this.loggerFactory.GetLogger(calcGuid);
            var collector = this.loggerFactory.GetCollector();
            var packetDomain = this.container.ResolveDomain<UnacceptedChargePacket>();
            try
            {
                indicator.Report(this, 95, "Обновление счётов начислений домов");
                logger.StartTimer("RealityObjectChargeAccountUpdate", "Пересчёт счёта начислений домов");
                this.UpdateRealityObjectChargeAccount(baseParams);
                logger.StopTimer("RealityObjectChargeAccountUpdate");

                logger.SaveLogs(collector, x => x.OrderByDescending(y => y.TimeSpan).First());
                logger.ClearSession();

                var packet = packetDomain.Get(packetId);
                if (packet.IsNotNull())
                {
                    packet.PacketState = PaymentOrChargePacketState.Accepted;
                    packetDomain.Update(packet);
                }
            }
            finally
            {
                this.container.Release(logger);
                this.container.Release(collector);
                this.container.Release(packetDomain);
            }

            return new CallbackResult(true);
        }

        private void UpdateRealityObjectChargeAccount(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
            var addressFilteredIds = this.personalAccountService.GetAccountIdsByAddress(loadParams);
            var currentPeriod = this.periodRepository.GetCurrentPeriod();

            var roIds = this.personalAccountDomain.GetAll()
                .ToDto()
                .WhereIf(ids != null && ids.Length > 0, x => ids.Contains(x.Id))
                .WhereIfContains(addressFilteredIds != null, x => x.Id, addressFilteredIds)
                .FilterByBaseParamsIf(ids?.Length == 0, baseParams, this.filterService)
                .FilterCalculable(currentPeriod, this.filterService)
                .FilterByRegFondSetting(this.filterService)
                .Filter(loadParams, this.container)
                .Select(x => x.RoId)
                .AsEnumerable()
                .Distinct()
                .ToArray();

            const string sqlUpdate = @"UPDATE regop_ro_charge_acc_charge ch
                    SET
                    ccharged = q.chargeTariff,
                    cpaid = q.paidTariff,
                    ccharged_penalty = q.chargePenalty,
                    cpaid_penalty = q.paidPenalty,
                    csaldo_in = q.saldoIn,
                    csaldo_out = q.saldoOut
                    FROM
                    (SELECT 
                    ch_acc.id acc_id,
                    ps.period_id,
                    SUM(ps.charge_tariff + ps.recalc + ps.recalc_decision + ps.balance_change + ps.dec_balance_change + ps.penalty_balance_change 
                        + ps.penalty + ps.recalc_penalty - ps.perf_work_charge) AS chargeTariff,
                    SUM(ps.penalty + ps.recalc_penalty + ps.penalty_balance_change) AS chargePenalty,
                    SUM(ps.tariff_payment + ps.tariff_desicion_payment) AS paidTariff,
                    SUM(ps.penalty_payment) AS paidPenalty,
                    SUM(ps.saldo_in) AS saldoIn,
                    SUM(ps.saldo_out) AS saldoOut
                    FROM regop_pers_acc_period_summ ps
                    JOIN regop_pers_acc ac ON ac.id = ps.account_id
                    JOIN gkh_room r ON r.id = ac.room_id
                    JOIN regop_ro_charge_account ch_acc ON ch_acc.ro_id = r.ro_id
                    WHERE ps.period_id = :period_id AND r.ro_id IN (:ro_ids)
                    GROUP BY ch_acc.id, ps.period_id) q
                    WHERE q.acc_id = ch.acc_id AND ch.period_id = q.period_id";

            var currentSession = this.sessionProvider.GetCurrentSession();
            foreach (var realityObjects in roIds.Section(1000))
            {
                currentSession.CreateSQLQuery(sqlUpdate).SetParameter("period_id", currentPeriod.Id).SetParameterList("ro_ids", realityObjects).ExecuteUpdate();
            }
        }
    }
}