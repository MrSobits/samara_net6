namespace Bars.Gkh.RegOperator.Tasks.Period.Executors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Repository;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Dapper;
    using NHibernate;
    using NHibernate.Linq;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Закрытие периода (Этап 1)
    /// </summary>
    public class PeriodCloseTaskExecutor_Step1 : ITaskExecutor
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        public PeriodCloseTaskExecutor_Step1(IWindsorContainer container)
        {
            this.container = container;
        }

        private void ProcessPortion(IQueryable<PersonalAccountCharge> portion, ChargePeriod period)
        {
            var sessionProvider = this.container.Resolve<ISessionProvider>();
            var periodSummaryDomain = this.container.ResolveDomain<PersonalAccountPeriodSummary>();
            var roChargeOperationDomain = this.container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var log = this.container.Resolve<ILogger>();
            var perfWorkDistribService = this.container.Resolve<IPerformedWorkDistribution>();
            var chargePeriod = period;
            var useNewPerfWorkCharges = this.container.GetGkhConfig<RegOperatorConfig>()
                .GeneralConfig.PerfWorkChargeConfig.PerfWorkChargeType == PerfWorkChargeType.ForNewCharges;

            log.LogDebug("Закрытие периода {0}", chargePeriod.ToJson());

            using (this.container.Using(sessionProvider, periodSummaryDomain, roChargeOperationDomain, perfWorkDistribService))
            {
                var transfers = new List<Transfer>();
                var operations = new List<MoneyOperation>();
                var accounts = new List<BasePersonalAccount>();

                var charges = portion
                    .Fetch(x => x.BasePersonalAccount)
                    .ThenFetch(x => x.BaseTariffWallet)
                    .Fetch(x => x.BasePersonalAccount)
                    .ThenFetch(x => x.DecisionTariffWallet)
                    .Fetch(x => x.BasePersonalAccount)
                    .ThenFetch(x => x.PenaltyWallet)
                    .ToArray();

                foreach (var charge in charges)
                {
                    var moneyOperation = charge.CreateOperation();
                    operations.Add(moneyOperation);

                    transfers.AddRange(charge.BasePersonalAccount.ApplyCharge(charge, moneyOperation));
                    accounts.Add(charge.BasePersonalAccount);
                }

                var calculatedparams = this.GetCalculatedParameters(sessionProvider, charges.Select(x => x.BasePersonalAccount.Id).ToArray());

                var count = 5000;

                TransactionHelper.InsertInManyTransactions(this.container, calculatedparams, count, true, true);
                TransactionHelper.InsertInManyTransactions(this.container, operations, count, true, true);
                TransactionHelper.InsertInManyTransactions(this.container, accounts, count, true, true);

                var connDb = sessionProvider.GetCurrentSession().Connection;
                using (var trans = connDb.BeginTransaction())
                {
                    try
                    {
                        var sql = $@" INSERT INTO public.regop_charge_transfer_period_{period.Id}
                                  (SOURCE_GUID,TARGET_GUID,TARGET_COEF,OP_ID,AMOUNT,REASON,
                                   ORIGINATOR_NAME,PAYMENT_DATE,OPERATION_DATE,IS_INDIRECT,
                                   ORIGINATOR_ID,IS_AFFECT,IS_LOAN,
                                   IS_RETURN_LOAN,PERIOD_ID, OWNER_ID,
                                   OBJECT_CREATE_DATE,OBJECT_EDIT_DATE,OBJECT_VERSION) VALUES 
                                  (@SourceGuid,@TargetGuid,@TargetCoef,@OperationId,@Amount,
                                   @Reason,@OriginatorName,@PaymentDate,@OperationDate,@IsInDirect,
                                   @OriginatorId,@IsAffect,@IsLoan,@IsReturnLoan,
                                   {period.Id},@OwnerId,@ObjectCreateDate,@ObjectEditDate,@ObjectVersion)";

                        foreach (var transfer in transfers)
                        {
                            connDb.Execute(
                                sql,
                                new
                                {
                                    transfer.SourceGuid,
                                    transfer.TargetGuid,
                                    transfer.TargetCoef,
                                    OperationId = transfer.Operation.Id,
                                    transfer.Amount,
                                    transfer.Reason,
                                    transfer.OriginatorName,
                                    transfer.PaymentDate,
                                    transfer.OperationDate,
                                    transfer.IsInDirect,
                                    OriginatorId = transfer.Originator?.Id,
                                    transfer.IsAffect,
                                    transfer.IsLoan,
                                    transfer.IsReturnLoan,
                                    OwnerId = transfer.Owner.Id,
                                    transfer.ObjectCreateDate,
                                    transfer.ObjectEditDate,
                                    transfer.ObjectVersion
                                },
                                trans);
                        }
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                    trans.Commit();
                }
                var chargeIds = charges.Select(x => x.Id).ToArray();

                if (chargeIds.Any())
                {
                    this.container.InTransaction(() =>
                    {
                        var session = sessionProvider.GetCurrentSession();
                        session.FlushMode = FlushMode.Auto;

                        if (useNewPerfWorkCharges)
                        {
                            session.Flush();
                            perfWorkDistribService.DistributeForAccountPacket(portion.Select(x => x.BasePersonalAccount));

                        }

                        session
                            .CreateQuery(" update PersonalAccountCharge p " +
                                "          set p.IsFixed = :isFixed " +
                                "          where ChargePeriod.Id=(:periodId) and Id in (:ids)")
                            .SetParameterList("ids", chargeIds)
                            .SetInt64("periodId", chargePeriod.Id)
                            .SetBoolean("isFixed", true)
                            .ExecuteUpdate();
                    });
                }
            }
        }

        private IEnumerable<PersonalAccountCalculatedParameter> GetCalculatedParameters(ISessionProvider sessionProvider, long[] accountIds)
        {
            if (accountIds.IsEmpty())
            {
                return new List<PersonalAccountCalculatedParameter>();
            }

            var tmpParams =
                sessionProvider
                    .GetCurrentSession()
                    .CreateQuery(
                        " select distinct al.PersonalAccount, al.LoggedEntity"
                            + " from PersonalAccountCalcParam_tmp al where al.PersonalAccount.Id in (:ids)")
                    .SetParameterList("ids", accountIds)
                    .List<object[]>();

            return tmpParams
                .Select(
                    x => new PersonalAccountCalculatedParameter
                    {
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        LoggedEntity = x[1].To<EntityLogLight>(),
                        PersonalAccount = x[0].To<BasePersonalAccount>()
                    })
                .ToList();
        }

        #region Implementation of ITaskExecutor
        /// <summary>
        /// Выполнить
        /// </summary>
        /// <param name="params">Параметры</param>
        /// <param name="ctx">Контекст</param>
        /// <param name="indicator">Индикатор</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Результат</returns>
        public IDataResult Execute(
            BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var periodId = @params.Params.GetAs<long>("periodId");
            var ids = @params.Params.GetAs<long[]>("ids");

            var provider = this.container.Resolve<ISessionProvider>();
            var periodDomain = this.container.ResolveDomain<ChargePeriod>();
            var chargeRepo = this.container.Resolve<IPersonalAccountChargeRepository>();

            var session = provider.GetCurrentSession();

            var oldFlushMode = session.FlushMode;
            session.FlushMode = FlushMode.Never;

            try
            {
                var period = periodDomain.Get(periodId);

                var needToBeUndoCharges = chargeRepo.GetNeedToBeUndo(period)
                    .Where(x => ids.Contains(x.Id));

                this.ProcessUndoChargeportion(needToBeUndoCharges, period);

                var needToBeFixed = chargeRepo.GetNeedToBeFixedForPeriod(period).Where(x => ids.Contains(x.Id));

                this.ProcessPortion(needToBeFixed, period);
            }
            finally
            {
                this.container.Release(provider);
                this.container.Release(periodDomain);
                this.container.Release(chargeRepo);

                session.Clear();
                session.FlushMode = oldFlushMode;
            }

            return new BaseDataResult();
        }

        private void ProcessUndoChargeportion(IQueryable<PersonalAccountCharge> charges, ChargePeriod period)
        {
            if (!charges.Any())
            {
                return;
            }

            this.container.Resolve<ISessionProvider>().InStatelessConnectionTransaction((conn, tr) =>
            {
                var portionAccountIds = charges.Select(x => x.BasePersonalAccount.Id).ToList();

                //получаем сальдовые строки за текущий период
                var sql =
                    $@" SELECT s.Id as Id, s.ACCOUNT_ID as PersonalAccountId, s.PERIOD_ID as PeriodId, CHARGE_TARIFF as ChargeTariff,
                                CHARGE_BASE_TARIFF as ChargedByBaseTariff, RECALC as RecalcByBaseTariff, RECALC_DECISION as RecalcByDecisionTariff,
                                RECALC_PENALTY as RecalcByPenalty,
                                PENALTY as Penalty, s.PENALTY_PAYMENT as PenaltyPayment, s.TARIFF_PAYMENT as TariffPayment,
                                s.SALDO_IN as SaldoIn, 0 as SaldoOut, s.TARIFF_DESICION_PAYMENT as TariffDecisionPayment, 
                                s.OVERHAUL_PAYMENT as OverhaulPayment, s.RECRUITMENT_PAYMENT as RecruitmentPayment, 
                                s.SALDO_IN_SERV as SaldoInFromServ, s.SALDO_OUT_SERV as SaldoOutFromServ, s.SALDO_CHANGE_SERV as SaldoChangeFromServ, 
                                s.BALANCE_CHANGE as BaseTariffChange, s.DEC_BALANCE_CHANGE as DecisionTariffChange, s.PENALTY_BALANCE_CHANGE as PenaltyChange,
                                s.BASE_TARIFF_DEBT as BaseTariffDebt, s.DEC_TARIFF_DEBT as DecisionTariffDebt, s.PENALTY_DEBT as PenaltyDebt, 
                                s.PERF_WORK_CHARGE as PerformedWorkCharged
                            FROM public.REGOP_PERS_ACC_PERIOD_SUMM_PERIOD_{period.Id} s 
                            WHERE s.ACCOUNT_ID IN ({string.Join(",", portionAccountIds)})";

                var accountSummaries = conn.Query<PersonalAccountPeriodSummaryDto>(sql, transaction: tr).ToDictionary(x => x.PersonalAccountId);

                foreach (var charge in charges)
                {
                    var accountSumm = accountSummaries[charge.BasePersonalAccount.Id];
                    accountSumm.UndoCharge(charge);

                    sql = $@"UPDATE public.regop_pers_acc_period_summ_period_{period.Id} 
                                    SET 
                                    CHARGE_TARIFF=@ChargeTariff, 
                                    CHARGE_BASE_TARIFF = @ChargedByBaseTariff,
                                    RECALC=@RecalcByBaseTariff, 
                                    RECALC_DECISION =@RecalcByDecisionTariff,
                                    RECALC_PENALTY=@RecalcByPenalty, 
                                    PENALTY = @Penalty, 
                                    SALDO_OUT = @SaldoOut, 
                                    PERF_WORK_CHARGE = @PerformedWorkCharged,
                                    PERF_WORK_CHARGE_DEC = @DecsicionPerformedWorkCharged,
                                    object_edit_date = current_timestamp
                                    WHERE ACCOUNT_ID=@PersonalAccountId;";

                    //обновляем текущие сальдовые строки в period_summary
                    conn.Execute(sql, accountSumm, transaction: tr);
                }
            });
        }

        /// <summary>
        /// Код
        /// </summary>
        public string ExecutorCode { get; private set; }
        #endregion
    }
}