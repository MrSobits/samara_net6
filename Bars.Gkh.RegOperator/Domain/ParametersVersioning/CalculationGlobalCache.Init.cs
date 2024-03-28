namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.ConfigSections.ClaimWork;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.Utils;

    using ConfigSections.RegOperator;

    using Dapper;

    using Dto;

    using Enums;

    using Newtonsoft.Json;

    using NHibernate.Linq;

    public partial class CalculationGlobalCache
    {
        private readonly IDictionary<long, DateTime> manualCalcDates = new Dictionary<long, DateTime>();
        /// <summary>
        /// Cписок причин трансферов полученных в результате расчета - не кэшируем
        /// </summary>
        private readonly List<string> listChargeReasonTransfers;

        public void SetManualRecalcDate(BasePersonalAccount account, DateTime date)
        {
            var minDate = this.manualCalcDates.Get<long, DateTime>(account.Id, DateTime.MaxValue);

            if (date.IsValid() && date < minDate)
            {
                minDate = date;
            }

            if (minDate.IsValid())
            {
                this.manualCalcDates[account.Id] = minDate;
            }
        }

        /// <summary>
        /// Сбор данных для расчет пени
        /// </summary>
        /// <param name="period">Период</param>
        /// <param name="accounts">Аккаунты ЛС</param>
        public void Initialize(IPeriod period, IQueryable<BasePersonalAccount> accounts)
        {
            lock (this._lock)
            {
                if (this.initialized)
                {
                    return;
                }

                this.performanceLogger.StartTimer("CalculationGlobalCache.InitInternal");
                this.InitInternal(accounts, period);
                this.performanceLogger.StopTimer("CalculationGlobalCache.InitInternal");

                var roIds = accounts.Select(y => y.Room.RealityObject.Id).ToArray().Distinct().ToArray();

                this.performanceLogger.StartTimer("CalculationGlobalCache.InitRoCache");
                this.InitRoCache(roIds);
                this.performanceLogger.StopTimer("CalculationGlobalCache.InitRoCache");

                this.performanceLogger.StartTimer("CalculationGlobalCache.entityLogCache.Init");
                this.entityLogCache.Init(accounts);
                this.performanceLogger.StopTimer("CalculationGlobalCache.entityLogCache.Init");

                this.performanceLogger.StartTimer("CalculationGlobalCache.tariffCache.Init");
                this.tariffCache.Init(roIds);
                this.performanceLogger.StopTimer("CalculationGlobalCache.tariffCache.Init");

                this.InitPenaltyParameters();
                this.initialized = true;
            }
        }

        private void InitInternal(IQueryable<BasePersonalAccount> accounts, IPeriod period)
        {
            var periodDomain = this.container.ResolveDomain<ChargePeriod>();
            var paymentPenaltiesDomain = this.container.ResolveDomain<PaymentPenalties>();
            var persAccDomain = this.container.ResolveDomain<BasePersonalAccount>();
            var monthDecisionDomain = this.container.ResolveDomain<MonthlyFeeAmountDecision>();
            var scheduleDomain = this.container.ResolveDomain<RestructDebtSchedule>();
            var penaltiesDeferredDomain = this.container.ResolveDomain<PenaltiesWithDeferred>();
            var banRecalcDomain = this.container.ResolveDomain<PersonalAccountBanRecalc>();
            var accountDetailDomain = this.container.ResolveDomain<ClaimWorkAccountDetail>();
            var perfWorkChargeSourceDomain = this.container.ResolveDomain<PerformedWorkChargeSource>();
            var fixPeriodCalcDomain = this.container.ResolveDomain<FixedPeriodCalcPenalties>();
            var restructDetailDomain = this.container.ResolveDomain<RestructDebtScheduleDetail>();

            var config = this.container.GetGkhConfig<DebtorClaimWorkConfig>();
            var sessionProvider = this.container.Resolve<ISessionProvider>();

            using (this.container.Using(sessionProvider))
            {
                var conn = sessionProvider.OpenStatelessSession().Connection;
                var sql = string.Empty;
                try
                {
                    var filter = accounts.Select(y => y.Id).ToArray();
                    var allPeriods = periodDomain.GetAll().ToList();

                    #region Period cache
                    this.periodCache = allPeriods.Where(x => x.IsClosed).ToHashSet();
                    #endregion

                    //получаем глубину перерасчета по каждому ЛС
                    var dictionaryRecalcPeriods = this.GetRecalcPeriods(period, accounts);

                    //максимальная глубина перерасчета 
                    var firstDayRecalc = dictionaryRecalcPeriods.Min(x => x.Value);

                    //список месяцев для расчета (включая текущий месяц)
                    var periodsForCalc = allPeriods.Where(x => x.EndDate >= firstDayRecalc || !x.IsClosed).OrderBy(x => x.StartDate);

                    List<long> Last2periods = new List<long>();
                    var calculatedPeriod = periodDomain.GetAll().Where(x => !x.IsClosed).FirstOrDefault().Id;
                    Last2periods.Add(calculatedPeriod);
                    Last2periods.Add(periodDomain.GetAll().Where(x => x.Id < calculatedPeriod).OrderByDescending(x => x.Id).FirstOrDefault().Id);

                    this.performanceLogger.StartTimer("walletGuids.transfers", "Получение кошельков ЛС");
                    //Получаем гуиды по трем видам кошельков
                    sql =
                        $@"SELECT p.id as PersonalAccountId,
                           MAX(CASE WHEN w.id=p.bt_wallet_id THEN w.wallet_guid END) as BaseTariffWalletGuid,
                           MAX(CASE WHEN w.id=p.dt_wallet_id THEN w.wallet_guid END) as DecisionTariffWalletGuid,
                           MAX(CASE WHEN w.id=p.p_wallet_id THEN w.wallet_guid END) as PenaltyWalletGuid
                           FROM  public.regop_wallet w JOIN public.regop_pers_acc p ON (w.id=p.bt_wallet_id OR w.id=p.dt_wallet_id OR w.id=p.p_wallet_id)
                           AND p.id IN ({string.Join(",", filter)}) 
                           GROUP BY PersonalAccountId;";
                    var persAccounts = conn.Query<WalletDto>(sql).ToList();
                    this.walletCache = persAccounts.ToDictionary(x => x.PersonalAccountId);

                    this.performanceLogger.StopTimer("walletGuids.transfers");

                    var accountChargeCacheList = new List<PersonalAccountChargeDto>();
                    var accountRecalcHistoryList = new List<RecalcHistoryDto>();
                    Dictionary<long, bool> accountPenaltyChargeDict = new Dictionary<long, bool>();
                    var accountSummariesCacheList = new List<PersonalAccountPeriodSummaryDto>();
                    var transferCacheList = new List<TransferDto>();
                    var cancelTransferCacheList = new List<TransferDto>();
                    var transferReturnCacheList = new List<TransferDto>();
                    var perfWorkCacheList = new List<TransferDto>();

                    var accList = accounts.Select(x => x.Id).ToArray();

                    sql =
                                $@" SELECT s.Id as Id, s.ACCOUNT_ID as PersonalAccountId, s.PERIOD_ID as PeriodId, 
                                 s.SALDO_IN as SaldoIn FROM public.REGOP_PERS_ACC_PERIOD_SUMM s 
                                WHERE s.ACCOUNT_ID IN ({string
                                    .Join(",", accList)}) AND PERIOD_ID IN ({string
                                    .Join(",", Last2periods.ToArray())})";
                   
                    var accountSummariesForSaldo = conn.Query<PersonalAccountPeriodSummaryDto>(sql).ToList();



                    accList.ForEach(
                          x =>
                          {
                              if (!accountPenaltyChargeDict.ContainsKey(x))
                              {
                                  var sumSaldo = accountSummariesForSaldo
                                  .Where(y=> y.PersonalAccountId == x)
                                    .Where(y => Last2periods.Contains(y.PeriodId))
                                    .Sum(y => y.SaldoIn);
                                  if (sumSaldo <= 0)
                                  {
                                      accountPenaltyChargeDict.Add(x, false);
                                  }
                                  else
                                  {
                                      accountPenaltyChargeDict.Add(x, true);
                                  }
                              }
                          }
                        );

                    this.performanceLogger.StartTimer("_accountChargeCache");
                    foreach (var currentPeriod in periodsForCalc.Where(x => x.IsClosed))
                    {
                        var periodId = currentPeriod.Id;

                        //список ЛС по которым нужно получить данные в перерасчетном периоде
                        var accountIds =
                            dictionaryRecalcPeriods.Where(x => currentPeriod.StartDate.WithLastDayMonth() >= x.Value)
                                .Select(x => x.Key)
                                .ToArray();

                        #region Account charges
                        sql =
                            $@"SELECT p.Id , p.PERS_ACC_ID as BasePersonalAccountId, p.CHARGE_DATE as ChargeDate, p.PERIOD_ID as ChargePeriodId, p.GUID as Guid,
                                p.CHARGE as Charge, p.CHARGE_TARIFF as ChargeTariff, p.PENALTY as Penalty, p.RECALC as RecalcByBaseTariff,
                                p.RECALC_DECISION as RecalcByDecisionTariff, p.RECALC_PENALTY as RecalcPenalty, p.OVERPLUS as OverPlus, p.IS_FIXED as IsFixed
                               FROM public.REGOP_PERS_ACC_CHARGE_PERIOD_{periodId} p
                               WHERE p.PERS_ACC_ID IN ({string.Join(",", accountIds)}) 
                                AND p.IS_FIXED=TRUE AND p.IS_ACTIVE=TRUE";

                        var accountChargeCachePart = conn.Query<PersonalAccountChargeDto>(sql);
                        accountChargeCacheList.AddRange(accountChargeCachePart);
                        #endregion
                    }
                    this.performanceLogger.StopTimer("_accountChargeCache");

                    foreach (var currentPeriod in periodsForCalc)
                    {
                        var periodId = currentPeriod.Id;

                        //список ЛС по которым нужно получить данные в перерасчетном периоде
                        var accountIds =
                            dictionaryRecalcPeriods.Where(x => currentPeriod.StartDate.WithLastDayMonth() >= x.Value)
                                .Select(x => x.Key)
                                .ToArray();

                        var walletGuids = persAccounts
                            .Where(x => accountIds.Contains(x.PersonalAccountId))
                            .SelectMany(x => new[] { x.BaseTariffWalletGuid.ToStringWithQuote(), x.DecisionTariffWalletGuid.ToStringWithQuote() })
                            .ToArray();


                        #region monthly data

                        #region Recalc history

                        if (currentPeriod.IsClosed)
                        {
                            // берем только те, которые связаны с подтвержденными начислениями, которые зафиксированы
                            sql =
                                $@"SELECT r.Id as Id, r.ACCOUNT_ID as PersonalAccountId, r.CALC_PERIOD_ID as CalcPeriodId, 
                                r.RECALC_PERIOD_ID as RecalcPeriodId, r.RECALC_SUM as RecalcSum, r.UNACCEPTED_GUID as UnacceptedChargeGuid,
                                r.RECALC_TYPE as RecalcType
                               FROM public.REGOP_RECALC_HISTORY_PERIOD_{periodId}  r 
                               WHERE r.recalc_type = {(int)RecalcType.Penalty}
                                AND r.calc_period_id<{period.Id}
                                AND r.ACCOUNT_ID IN ({string.Join(",", accountIds)})";
                            this.performanceLogger.StartTimer("accountRecalcHistory");
                            var accountRecalcHistoryPart = conn.Query<RecalcHistoryDto>(sql).ToList();
                            accountRecalcHistoryPart.ForEach(
                                x =>
                                {
                                    x.RecalcPeriod = allPeriods.FirstOrDefault(z => z.Id == x.RecalcPeriodId);
                                    x.CalcPeriod = allPeriods.FirstOrDefault(z => z.Id == x.CalcPeriodId);
                                });
                            accountRecalcHistoryList.AddRange(accountRecalcHistoryPart);
                            this.performanceLogger.StopTimer("accountRecalcHistory");
                        }

                        #endregion

                        #region Account summaries
                        if (currentPeriod.IsClosed)
                        {
                            sql =
                                $@" SELECT s.Id as Id, s.ACCOUNT_ID as PersonalAccountId, s.PERIOD_ID as PeriodId, s.CHARGE_TARIFF as ChargeTariff,
                                 s.CHARGE_BASE_TARIFF as ChargedByBaseTariff, s.RECALC as RecalcByBaseTariff, s.RECALC_DECISION as RecalcByDecisionTariff,
                                 s.RECALC_PENALTY as RecalcByPenalty,
                                 s.PENALTY as Penalty, s.PENALTY_PAYMENT as PenaltyPayment, s.TARIFF_PAYMENT as TariffPayment,
                                 s.SALDO_IN as SaldoIn, s.SALDO_OUT as SaldoOut, s.TARIFF_DESICION_PAYMENT as TariffDecisionPayment, 
                                 s.OVERHAUL_PAYMENT as OverhaulPayment, s.RECRUITMENT_PAYMENT as RecruitmentPayment, 
                                 s.SALDO_IN_SERV as SaldoInFromServ, s.SALDO_OUT_SERV as SaldoOutFromServ, s.SALDO_CHANGE_SERV as SaldoChangeFromServ, 
                                 s.BALANCE_CHANGE as BaseTariffChange, s.DEC_BALANCE_CHANGE as DecisionTariffChange, s.PENALTY_BALANCE_CHANGE as PenaltyChange,
                                 s.BASE_TARIFF_DEBT as BaseTariffDebt, s.DEC_TARIFF_DEBT as DecisionTariffDebt, s.PENALTY_DEBT as PenaltyDebt, 
                                 s.PERF_WORK_CHARGE as PerformedWorkCharged
                                FROM public.REGOP_PERS_ACC_PERIOD_SUMM_PERIOD_{periodId} s 
                                WHERE s.ACCOUNT_ID IN ({string
                                    .Join(",", accountIds)})";
                            this.performanceLogger.StartTimer("accountSummariesCache");
                            var accountSummariesCachePart = conn.Query<PersonalAccountPeriodSummaryDto>(sql).ToList();                        
                           
                            accountSummariesCachePart.ForEach(
                                x =>
                                {
                                    x.Period = allPeriods.FirstOrDefault(z => z.Id == x.PeriodId);
                                   
                                });
                            accountSummariesCacheList.AddRange(accountSummariesCachePart);
                            this.performanceLogger.StopTimer("accountSummariesCache");
                        }
                        #endregion

                        #region Transfers

                        var regopTransferTable = $"public.REGOP_TRANSFER_PERIOD_{periodId}";
                        var regopChargeTransferTable = $"public.REGOP_CHARGE_TRANSFER_PERIOD_{periodId}";
                        var regopMoneyOperation = "public.REGOP_MONEY_OPERATION";
                        var paymentOperationBase = "public.REGOP_PAYMENT_OPERATION_BASE";
                        sql =
                            $@"SELECT t.ID as Id, t.SOURCE_GUID as SourceGuid, t.TARGET_GUID as TargetGuid, t.TARGET_COEF as TargetCoef,
                               t.OP_ID as OperationId, t.AMOUNT as Amount, t.REASON as Reason, t.ORIGINATOR_NAME as OriginatorName,
                               t.PAYMENT_DATE as PaymentDate, t.OPERATION_DATE as OperationDate, t.IS_INDIRECT as IsInDirect, 
                               t.IS_AFFECT as IsAffect, t.IS_LOAN as IsLoan, t.IS_RETURN_LOAN as IsReturnLoan, 
                               t.PERIOD_ID as ChargePeriodId, coalesce(p.PAYMENT_SOURCE,0) as PaymentSource,     
                               m.Id as Id, m.CANCELED_OP_ID as CanceledOperationId, m.OP_GUID as OperationGuid, m.ORIGINATOR_GUID as OriginatorGuid,
                               m.IS_CANCELLED as IsCancelled, m.AMOUNT  as Amount, m.REASON as Reason, m.DOCUMENT_ID as DocumentId, m.OPERATION_DATE as OperationDate
                               FROM {regopTransferTable} t 
                                JOIN {regopMoneyOperation} m ON t.OP_ID=m.Id 
                                LEFT JOIN {paymentOperationBase} p ON t.SOURCE_GUID=p.GUID
                                WHERE t.PERIOD_ID={periodId} 
                                AND t.IS_AFFECT
                                AND t.OWNER_ID in ({string.Join(",", accountIds)}) AND t.TARGET_GUID IN ({string.Join(",", walletGuids)})";
                        this.performanceLogger.StartTimer("transferCache");
                        var transferCachePart = conn.Query<TransferDto, MoneyOperationDto, TransferDto>(
                            sql,
                            (x, y) =>
                            {
                                x.Operation = y;
                                return x;
                            });
                        transferCacheList.AddRange(transferCachePart);
                        this.performanceLogger.StopTimer("transferCache");

                        //получаем переносы долга с других ЛС
                        sql =
                          $@"SELECT t.ID as Id, t.SOURCE_GUID as SourceGuid, t.TARGET_GUID as TargetGuid, t.TARGET_COEF as TargetCoef,
                               t.OP_ID as OperationId, t.AMOUNT as Amount, t.REASON as Reason, t.ORIGINATOR_NAME as OriginatorName,
                               t.PAYMENT_DATE as PaymentDate, t.OPERATION_DATE as OperationDate, t.IS_INDIRECT as IsInDirect, 
                               t.IS_AFFECT as IsAffect, t.IS_LOAN as IsLoan, t.IS_RETURN_LOAN as IsReturnLoan, 
                               t.PERIOD_ID as ChargePeriodId, 
                               m.Id as Id, m.CANCELED_OP_ID as CanceledOperationId, m.OP_GUID as OperationGuid, m.ORIGINATOR_GUID as OriginatorGuid,
                               m.IS_CANCELLED as IsCancelled, m.AMOUNT  as Amount, m.REASON as Reason, m.DOCUMENT_ID as DocumentId, m.OPERATION_DATE as OperationDate                               
                               FROM {regopTransferTable} t 
                                JOIN {regopMoneyOperation} m ON t.OP_ID=m.Id 
                                WHERE t.PERIOD_ID={periodId} 
                                AND t.REASON IS NULL
                                AND t.OWNER_ID in ({string.Join(",", accountIds)}) AND t.TARGET_GUID IN ({string.Join(",", walletGuids)})";
                        this.performanceLogger.StartTimer("transferFromOtherCache");
                        var transferFromOtherAccountList = conn.Query<TransferDto, MoneyOperationDto, TransferDto>(
                            sql,
                            (x, y) =>
                            {
                                x.Operation = y;
                                return x;
                            });
                        transferCacheList.AddRange(transferFromOtherAccountList);
                        this.performanceLogger.StopTimer("transferFromOtherCache");

                        //попробуем отмены за несколько периодов
                        sql =
                        $@"SELECT c.account_id as PersonalAccountId,c.charge_op_id as ChargeOperationId, 
                                        c.cancel_period_id as CancelPeriodId,c.cancel_type as CancelType,c.cancel_sum as CancelSum 
                                FROM  public.regop_cancel_charge c JOIN public.regop_charge_operation_base b    
                                ON c.charge_op_id=b.id AND b.period_id={periodId} 
                                   AND c.account_id in ({string.Join(",", filter)})";
                        var cancelsForPeriod = conn.Query<CancelChargeDto>(sql).GroupBy(x => x.PersonalAccountId).ToDictionary(x => x.Key, x => x.ToArray());
                        cancelsForPeriod.ForEach(x =>
                        {
                            if (this.allcancelChargeCache.ContainsKey(x.Key))
                            {
                                var oldList = allcancelChargeCache[x.Key].ToList();
                                oldList.AddRange(x.Value.ToList());
                                this.allcancelChargeCache[x.Key] = oldList.ToArray();
                            }
                            else
                            {
                                this.allcancelChargeCache.Add(x.Key, x.Value);

                            }
                        });



                        sql =
                           $@"SELECT t.ID as Id, t.SOURCE_GUID as SourceGuid, t.TARGET_GUID as TargetGuid, t.TARGET_COEF as TargetCoef,
                               t.OP_ID as OperationId, t.AMOUNT as Amount, t.REASON as Reason, t.ORIGINATOR_NAME as OriginatorName,
                               t.PAYMENT_DATE as PaymentDate, t.OPERATION_DATE as OperationDate, t.IS_INDIRECT as IsInDirect, 
                               t.IS_AFFECT as IsAffect, t.IS_LOAN as IsLoan, t.IS_RETURN_LOAN as IsReturnLoan, 
                               t.PERIOD_ID as ChargePeriodId, 
                               m.Id, m.CANCELED_OP_ID as CanceledOperationId, m.OP_GUID as OperationGuid, m.ORIGINATOR_GUID as OriginatorGuid,
                               m.IS_CANCELLED as IsCancelled, m.AMOUNT  as Amount, m.REASON as Reason, m.DOCUMENT_ID as DocumentId, m.OPERATION_DATE as OperationDate                               
                               FROM {regopTransferTable} t JOIN {regopMoneyOperation} m ON t.OP_ID=m.Id 
                               WHERE t.PERIOD_ID={periodId} 
                               AND t.IS_AFFECT                    
                               AND t.OWNER_ID in ({string.Join(",", accountIds)}) AND t.SOURCE_GUID IN ({string.Join(",", walletGuids)}) 
                               AND m.CANCELED_OP_ID IS NOT NULL;";
                        this.performanceLogger.StartTimer("cancelTransferCache");
                        var cancelTransferCachePart = conn.Query<TransferDto, MoneyOperationDto, TransferDto>(
                            sql,
                            (x, y) =>
                            {
                                x.Operation = y;
                                return x;
                            });
                        cancelTransferCacheList.AddRange(cancelTransferCachePart);
                        this.performanceLogger.StopTimer("cancelTransferCache");

                        this.performanceLogger.StartTimer("perfWorkCache");
                        sql =
                            $@"SELECT t.ID as Id, t.SOURCE_GUID as SourceGuid, t.TARGET_GUID as TargetGuid, t.TARGET_COEF as TargetCoef,
                               t.OP_ID as OperationId, t.AMOUNT as Amount, t.REASON as Reason, t.ORIGINATOR_NAME as OriginatorName,
                               t.PAYMENT_DATE as PaymentDate, t.OPERATION_DATE as OperationDate, t.IS_INDIRECT as IsInDirect, 
                               t.IS_AFFECT as IsAffect, t.IS_LOAN as IsLoan, t.IS_RETURN_LOAN as IsReturnLoan, 
                               t.PERIOD_ID as ChargePeriodId
                               FROM {regopChargeTransferTable} t
                               WHERE t.period_id={periodId}
                               AND not t.is_affect
                               AND t.reason='Зачет средств за выполненные работы'
                               AND t.OWNER_ID in ({string.Join(",", accountIds)}) and t.source_guid in ({string.Join(",", walletGuids)})
                               and not exists (select 1 from regop_charge_operation_base co where co.guid=t.target_guid);";

                        var perfWorkPart = conn.Query<TransferDto>(sql);
                        perfWorkCacheList.AddRange(perfWorkPart);
                        this.performanceLogger.StopTimer("perfWorkCache");

                        sql =
                           $@"SELECT t.ID as Id, t.SOURCE_GUID as SourceGuid, t.TARGET_GUID as TargetGuid, t.TARGET_COEF as TargetCoef,
                               t.OP_ID as OperationId, t.AMOUNT as Amount, t.REASON as Reason, t.ORIGINATOR_NAME as OriginatorName,
                               t.PAYMENT_DATE as PaymentDate, t.OPERATION_DATE as OperationDate, t.IS_INDIRECT as IsInDirect, 
                               t.IS_AFFECT as IsAffect, t.IS_LOAN as IsLoan, t.IS_RETURN_LOAN as IsReturnLoan, 
                               t.PERIOD_ID as ChargePeriodId, 
                               m.Id, m.CANCELED_OP_ID as CanceledOperationId, m.OP_GUID as OperationGuid, m.ORIGINATOR_GUID as OriginatorGuid,
                               m.IS_CANCELLED as IsCancelled, m.AMOUNT  as Amount, m.REASON as Reason, m.DOCUMENT_ID as DocumentId, m.OPERATION_DATE as OperationDate                               
                               FROM {regopTransferTable} t JOIN {regopMoneyOperation} m ON t.OP_ID=m.Id 
                               WHERE t.PERIOD_ID={periodId} 
                               AND t.IS_AFFECT
                               AND t.REASON LIKE 'Возврат%'
                               AND t.OWNER_ID in ({string.Join(",", accountIds)}) and t.SOURCE_GUID IN ({string.Join(",", walletGuids)})  
                               AND m.CANCELED_OP_ID IS NULL
                               AND not m.IS_CANCELLED;";
                        this.performanceLogger.StartTimer("transferReturnCache");
                        var transferReturnCachePart = conn.Query<TransferDto, MoneyOperationDto, TransferDto>(
                            sql,
                            (x, y) =>
                            {
                                x.Operation = y;
                                return x;
                            });
                        transferReturnCacheList.AddRange(transferReturnCachePart);
                        this.performanceLogger.StopTimer("transferReturnCache");
                        #endregion Transfers

                        #endregion monthly data
                    }

                    //список ЛС с перерасчетами по тарифам, не по пени
                    var listPersonalAccountIdsWithRecalcs =
                        accountSummariesCacheList.Where(x => x.RecalcByBaseTariff != 0 || x.RecalcByDecisionTariff != 0)
                            .Select(x => x.PersonalAccountId).Distinct()
                            .ToList();

                    if (listPersonalAccountIdsWithRecalcs.Any())
                    {
                        sql = $@"SELECT r.Id as Id, r.ACCOUNT_ID as PersonalAccountId, r.CALC_PERIOD_ID as CalcPeriodId, 
                                r.RECALC_PERIOD_ID as RecalcPeriodId, r.RECALC_SUM as RecalcSum, r.UNACCEPTED_GUID as UnacceptedChargeGuid,
                                r.RECALC_TYPE as RecalcType
                               FROM public.REGOP_RECALC_HISTORY_CHARGE r 
                               WHERE r.recalc_type IN ({(int)RecalcType.BaseTariffCharge},{(int)RecalcType.DecisionTariffCharge})
                                AND r.ACCOUNT_ID IN ({string.Join(",", listPersonalAccountIdsWithRecalcs)})";
                        var accountRecalcHistoryOnCharge = conn.Query<RecalcHistoryDto>(sql).ToList();
                        accountRecalcHistoryOnCharge.ForEach(x =>
                        {
                            x.RecalcPeriod = allPeriods.FirstOrDefault(z => z.Id == x.RecalcPeriodId);
                            x.CalcPeriod = allPeriods.FirstOrDefault(z => z.Id == x.CalcPeriodId);
                        });
                        accountRecalcHistoryList.AddRange(accountRecalcHistoryOnCharge);
                    }

                    //получаем отмены начислений и группируем по типам - понадобятся при подсчете period_summary
                    sql =
                        $@"SELECT c.account_id as PersonalAccountId,c.charge_op_id as ChargeOperationId, 
                                        c.cancel_period_id as CancelPeriodId,c.cancel_type as CancelType,c.cancel_sum as CancelSum 
                                FROM  public.regop_cancel_charge c JOIN public.regop_charge_operation_base b    
                                ON c.charge_op_id=b.id AND b.period_id={period.Id} 
                                   AND c.account_id in ({string.Join(",", filter)})";
                    this.cancelChargeCache = conn.Query<CancelChargeDto>(sql).GroupBy(x => x.PersonalAccountId).ToDictionary(x => x.Key, x => x.ToArray());


                    var allWalletGuids =
                        persAccounts.SelectMany(
                            x =>
                                new[]
                                {
                                    x.BaseTariffWalletGuid.ToStringWithQuote(),
                                    x.DecisionTariffWalletGuid.ToStringWithQuote(),
                                    x.PenaltyWalletGuid.ToStringWithQuote()
                                })
                            .ToArray();

                    //получаем переносы долга с других ЛС за текущий период по всем кошелькам
                    sql =
                      $@"SELECT t.SOURCE_GUID as SourceGuid, t.TARGET_GUID as TargetGuid, t.TARGET_COEF as TargetCoef,
                               t.OP_ID as OperationId, t.AMOUNT as Amount, t.REASON as Reason, t.ORIGINATOR_NAME as OriginatorName,
                               t.PAYMENT_DATE as PaymentDate, t.OPERATION_DATE as OperationDate, t.IS_INDIRECT as IsInDirect, 
                               t.IS_AFFECT as IsAffect, t.IS_LOAN as IsLoan, t.IS_RETURN_LOAN as IsReturnLoan, 
                               t.PERIOD_ID as ChargePeriodId, 
                               m.Id as Id, m.CANCELED_OP_ID as CanceledOperationId, m.OP_GUID as OperationGuid, m.ORIGINATOR_GUID as OriginatorGuid,
                               m.IS_CANCELLED as IsCancelled, m.AMOUNT  as Amount, m.REASON as Reason, m.DOCUMENT_ID as DocumentId, m.OPERATION_DATE as OperationDate                               
                               FROM public.regop_charge_transfer_period_{period.Id} t JOIN public.regop_money_operation m ON t.OP_ID=m.Id 
                               WHERE t.REASON IS NULL AND t.AMOUNT>0
                               AND t.OWNER_ID in ({string.Join(",", filter)}) and t.TARGET_GUID IN ({string.Join(",", allWalletGuids)})";
                    this.transferFromOtherAccountsCache = conn.Query<TransferDto, MoneyOperationDto, TransferDto>(
                        sql,
                        (x, y) =>
                        {
                            x.Operation = y;
                            return x;
                        }).GroupBy(x => x.TargetGuid).ToDictionary(x => x.Key, y => y.ToArray());


                    this.accountChargeCache = accountChargeCacheList.GroupBy(x => x.BasePersonalAccountId).ToDictionary(x => x.Key, x => x.ToArray());
                    this.accountRecalcHistory = accountRecalcHistoryList.GroupBy(x => x.PersonalAccountId).ToDictionary(x => x.Key, x => x.ToArray());
                    this.accountSummariesCache = accountSummariesCacheList.GroupBy(x => x.PersonalAccountId).ToDictionary(x => x.Key, x => x.ToArray());
                    this.transferCache = transferCacheList.GroupBy(x => x.TargetGuid).ToDictionary(x => x.Key, y => y.ToArray());
                    this.cancelTransferCache = cancelTransferCacheList.GroupBy(x => x.SourceGuid).ToDictionary(x => x.Key, y => y.ToArray());
                    this.transferReturnCache = transferReturnCacheList.GroupBy(x => x.SourceGuid).ToDictionary(x => x.Key, y => y.ToArray());
                    this.perfWorkCache = perfWorkCacheList.GroupBy(x => x.SourceGuid).ToDictionary(x => x.Key, y => y.ToArray());

                    this.schedules = scheduleDomain.GetAll()
                        .Fetch(x => x.RestructDebt)
                        .Where(x => !x.RestructDebt.ClaimWork.IsDebtPaid)
                        .Where(x => x.RestructDebt.DocumentType == ClaimWorkDocumentType.RestructDebt)
                        .Where(x => x.RestructDebt.DocumentState == RestructDebtDocumentState.Active)
                        .Where(x => filter.Contains(x.PersonalAccount.Id))
                        .AsEnumerable()
                        .GroupBy(x => x.PersonalAccount.Id)
                        .ToDictionary(x => x.Key, x => x.ToArray());

                    this.detailSchedules = restructDetailDomain.GetAll()
                        .Fetch(x => x.ScheduleRecord)
                        .Where(x => filter.Contains(x.ScheduleRecord.PersonalAccount.Id))
                        .Where(x => x.ScheduleRecord.RestructDebt.DocumentType == ClaimWorkDocumentType.RestructDebt)
                        .Where(x => x.ScheduleRecord.RestructDebt.DocumentState == RestructDebtDocumentState.Active)
                        .AsEnumerable()
                        .GroupBy(x => x.ScheduleRecord.PersonalAccount.Id)
                        .ToDictionary(x => x.Key, x => x.ToArray());

                    this.penaltyParamCache = paymentPenaltiesDomain.GetAll()
                        .Where(x => x.DateStart <= period.GetEndDate())
                        .ToHashSet();

                    this.monthlyDecisions =
                        monthDecisionDomain.GetAll()
                            .Where(x => x.Protocol.State.FinalState)
                            .Where(x => x.Decision != null)
                            .Select(
                                x => new
                                {
                                    x.Protocol.RealityObject.Id,
                                    MonthlyFeeAmountDecision = x
                                })
                            .AsEnumerable()
                            .GroupBy(x => x.Id)
                            .ToDictionary(x => x.Key, x => x.Select(y => y.MonthlyFeeAmountDecision).ToArray());

                    this.performanceLogger.StartTimer("walletGuids.transfers", "Guid кошельков для получения трансферов в кэше");

                    this.accountBanRecalc = banRecalcDomain.GetAll()
                        .Where(x => filter.Contains(x.PersonalAccount.Id))
                        .GroupBy(x => x.PersonalAccount.Id)
                        .ToDictionary(x => x.Key, x => x.ToArray());

                    this.fixperCalcPenaltieses = fixPeriodCalcDomain.GetAll().ToList();
                        
                    this.perfWorkCharge = perfWorkChargeSourceDomain.GetAll()
                        .Where(x => filter.Contains(x.PersonalAccount.Id))
                        .GroupBy(x => x.PersonalAccount.Id)
                        .ToDictionary(
                            x => x.Key,
                            y => y.SelectMany(z => z.PerformedWorkCharges).Where(x => x.Active).ToArray());

                    this.DatePenaltyCalcTo =
                        penaltiesDeferredDomain.GetAll().OrderByDescending(x => x.DateStartCalc).Select(x => x.DateEndCalc).FirstOrDefault();
                    this.DatePenaltyCalcFrom =
                        penaltiesDeferredDomain.GetAll().OrderByDescending(x => x.DateStartCalc).Select(x => x.DateStartCalc).FirstOrDefault();

                    this.IsClaimWorkEnabled = this.container.GetGkhConfig<ClaimWorkConfig>().Enabled;
                    this.CalculatePenalty = this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.CalculatePenalty;
                    this.RecalcPenaltyByCurrentRefinancingRate = this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.RecalcPenaltyByCurrentRefinancingRate;
                    this.CalcPenByAccId = accountPenaltyChargeDict;
                    this.SimpleCalculatePenalty =
                        this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.PenaltyCalcConfig.SimpleCalculatePenalty;
                    this.CalcPenaltyOneTimeMunicipalProperty = this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.CalculatePenaltyMunicipalPropertyOnePerYear;
                    this.NumberDaysDelay = this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.NumberDaysDelay;
                    this.CalculatePenaltyForDecisionTarif =
                        this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.CalculatePenaltyForDecisionTarif;
                    this.NewPenaltyCalcStart =
                        this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.NewPenaltyCalcConfig.NewPenaltyCalcStart;
                    this.NewPenaltyCalcDays =
                        this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.NewPenaltyCalcConfig.NewPenaltyCalcDays;
                    this.IsFixCalcPeriod =
                        this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.FixedPeriodCalcPenaltiesConfig.UseFixCalcPeriod;
                    this.RefinancingRate = this.container.GetGkhConfig<RegOperatorConfig>().PaymentPenaltiesNodeConfig.RefinancingRate;
                    this.IndividualAllowDelayPaymentDays = this.container.GetGkhConfig<DebtorClaimWorkConfig>().Individual.RestructDebt.AllowDelayPaymentDays;
                    this.LegalAllowDelayPaymentDays = this.container.GetGkhConfig<DebtorClaimWorkConfig>().Legal.RestructDebt.AllowDelayPaymentDays;
                }
                finally
                {
                    this.container.Release(periodDomain);
                    this.container.Release(paymentPenaltiesDomain);
                    this.container.Release(persAccDomain);
                    this.container.Release(monthDecisionDomain);
                    this.container.Release(fixPeriodCalcDomain);
                    this.container.Release(banRecalcDomain);
                    this.container.Release(accountDetailDomain);
                }
            }
        }

        /// <summary>
        /// Определение глубины перерасчета для лицевых счетов
        /// </summary>
        /// <param name="period"></param>
        /// <param name="accounts"></param>
        /// <returns></returns>
        private Dictionary<long, DateTime> GetRecalcPeriods(IPeriod period, IQueryable<BasePersonalAccount> accounts)
        {
            var accountChangeDomain = this.container.ResolveDomain<PersonalAccountChange>();
            var eventDomain = this.container.ResolveDomain<PersonalAccountRecalcEvent>();
            try
            {
                this.performanceLogger.StartTimer("GetRecalcPeriods", "Получение глубины перерасчета");
                var filter = accounts.Select(x => x.Id).ToArray();

                var dictionaryRecalcPeriods = new Dictionary<long, DateTime>();
                foreach (var account in filter)
                {
                    dictionaryRecalcPeriods[account] = period.StartDate;
                }

                var periodsRecalcByEntityLog = this.entityLogCache.GetStartDateRecalc(accounts, (ChargePeriod)period);

                this.manuallyRecalcDates = accountChangeDomain.GetAll()
                    .Where(x => x.ChangeType == PersonalAccountChangeType.ManuallyRecalc)
                    .Where(x => x.ChargePeriod.Id == period.Id)
                    .Where(x => filter.Contains(x.PersonalAccount.Id))
                    .Select(
                        x => new
                        {
                            x.PersonalAccount.Id,
                            x.ActualFrom,
                            x.Date
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, z => z.OrderBy(y=> y.Date).Select(y => y.ActualFrom).First());

                this.recalcEvents = eventDomain.GetAll()
                    .Where(x => filter.Contains(x.PersonalAccount.Id) || x.PersonalAccount == null)
                    .ToList()
                    .GroupBy(x => x.PersonalAccount?.Id ?? 0L)
                    .ToDictionary(x => x.Key, x => x.ToList());

                var dictionaryEvents =
                    this.recalcEvents.OrderBy(x => x.Key).ToDictionary(x => x.Key, pair => pair.Value.Min(z => z.EventDate));

                foreach (var pair in this.manuallyRecalcDates)
                {
                    if (dictionaryRecalcPeriods.ContainsKey(pair.Key)
                        && dictionaryRecalcPeriods[pair.Key] > pair.Value && pair.Value.HasValue)
                    {
                        dictionaryRecalcPeriods[pair.Key] = pair.Value.Value;
                    }
                }

                foreach (var pair in dictionaryEvents)
                {
                    if (pair.Key == 0) //признак для всех ЛС
                    {
                        foreach (var accountId in filter)
                        {
                            if (dictionaryRecalcPeriods[accountId] > pair.Value)
                            {
                                dictionaryRecalcPeriods[accountId] = pair.Value;
                            }
                        }
                    }

                    if (dictionaryRecalcPeriods.ContainsKey(pair.Key)
                        && dictionaryRecalcPeriods[pair.Key] > pair.Value)
                    {
                        dictionaryRecalcPeriods[pair.Key] = pair.Value;
                    }
                }

                foreach (var pair in periodsRecalcByEntityLog)
                {
                    if (dictionaryRecalcPeriods.ContainsKey(pair.Key)
                        && dictionaryRecalcPeriods[pair.Key] > pair.Value)
                    {
                        dictionaryRecalcPeriods[pair.Key] = pair.Value;
                    }
                }

                foreach (var pair in this.manualCalcDates)
                {
                    if (dictionaryRecalcPeriods.ContainsKey(pair.Key)
                        && dictionaryRecalcPeriods[pair.Key] > pair.Value)
                    {
                        dictionaryRecalcPeriods[pair.Key] = pair.Value;
                    }
                }

                var firstCalcPeriod = this.periodCache.OrderBy(x => x.StartDate).First();

                dictionaryRecalcPeriods =
                    dictionaryRecalcPeriods.Select(
                        x =>
                            new KeyValuePair<long, DateTime>(
                                x.Key,
                                x.Value <= firstCalcPeriod.StartDate ? firstCalcPeriod.StartDate
                                //искусственно увеличиваем глубину перерасчета - для расчета пеней
                                : x.Value.AddMonths(-3)))
                        .ToDictionary(x => x.Key, z => z.Value);
                return dictionaryRecalcPeriods;
            }
            finally
            {
                this.performanceLogger.StopTimer("GetRecalcPeriods");
                this.container.Release(eventDomain);
                this.container.Release(accountChangeDomain);
            }
        }

        private void InitRoCache(long[] roIds)
        {
            var service = this.container.Resolve<IRealityObjectDecisionsService>();

            try
            {
                this.roCrFundCache = service.GetRobjectsFundFormationForRecalc(roIds);

                var sql =
                    $@"SELECT realityobj1_.RO_ID as ""RoId"", realityobj1_.DATE_START as ""StartDate"", penaltydel0_.DECISION as ""Decision"" 
                             FROM DEC_PENALTY_DELAY penaltydel0_ 
                             INNER JOIN DEC_ULTIMATE_DECISION penaltydel0_1_ ON penaltydel0_.Id=penaltydel0_1_.Id 
                             INNER JOIN GKH_OBJ_D_PROTOCOL realityobj1_ ON penaltydel0_1_.PROTOCOL_ID=realityobj1_.Id 
                             INNER JOIN B4_STATE state2_ ON realityobj1_.STATE_ID=state2_.ID 
                             WHERE realityobj1_.RO_ID in ({string.Join(",", roIds)}) 
                                AND state2_.FINAL_STATE=TRUE 
                                AND (penaltydel0_.DECISION is not null)";

                var sessionProvider = this.container.Resolve<ISessionProvider>();
                using (this.container.Using(sessionProvider))
                {
                    var conn = sessionProvider.OpenStatelessSession().Connection;
                    var penaltyDelayDecisionDtoList = conn.Query<PenaltyDelayDecisionDto>(sql).ToList();
                    penaltyDelayDecisionDtoList
                        .ForEach(x => x.PenaltyDelays = JsonConvert.DeserializeObject<List<OwnerPenaltyDelay>>(x.Decision));

                    this.ownerDelayParams = penaltyDelayDecisionDtoList
                        .GroupBy(x => x.RoId)
                        .ToDictionary(
                            x => x.Key,
                            z => z
                                .Select(x => new Tuple<DateTime, List<OwnerPenaltyDelay>>(x.StartDate, x.PenaltyDelays))
                                .ToList());
                }
            }
            finally
            {
                this.container.Release(service);
            }
        }

        /// <summary>
        /// Инициализация ставки рефинансирования и количества дней допустимой просрочки.
        /// <remarks>Здесь происходит слияние периодов действия параметров, если параметр не изменился</remarks>
        /// </summary>
        private void InitPenaltyParameters()
        {
            this.penaltyPercentages = this.FillParams(this.penaltyParamCache, y => y.Percentage);
            this.penaltyDebtDays = this.FillParams(this.penaltyParamCache, y => y.Days);
        }

        private List<PenaltyParameterValue<TValue>> FillParams<TValue>(IEnumerable<PaymentPenalties> parameters, Func<PaymentPenalties, TValue> selector)
        {
            var percentageResult = new List<PenaltyParameterValue<TValue>>();

            foreach (var parameterValue in parameters.OrderBy(x => x.DateStart))
            {
                var lastParam = percentageResult.LastOrDefault(x => x.FundFormationType == parameterValue.DecisionType);
                var value = selector(parameterValue);

                if (lastParam != null && lastParam.Value.Equals(value))
                {
                    lastParam.SetEndDate(parameterValue);
                }
                else
                {
                    percentageResult.Add(this.CreatePenaltyParameter(parameterValue, value));
                }
            }
            return percentageResult;
        }

        private PenaltyParameterValue<TValue> CreatePenaltyParameter<TValue>(PaymentPenalties penalty, TValue value)
        {
            return new PenaltyParameterValue<TValue>
            {
                DateStart = penalty.DateStart,
                FundFormationType = penalty.DecisionType,
                DateEnd = penalty.DateEnd,
                Value = value,
                Source = new List<PaymentPenalties> { penalty }
            };
        }
    }
}
