namespace Bars.Gkh.RegOperator.Tasks.Charges.Executors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using B4;
    using B4.Application;
    using B4.DataAccess;
    using B4.IoC;
    using Microsoft.Extensions.Logging;
    using B4.Modules.Tasks.Common.Service;
    using B4.Modules.Tasks.Common.Utils;
    using B4.Utils;

    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Distribution;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.PerformanceLogging;

    using Castle.MicroKernel;

    using Dapper;

    using Decisions.Nso.Domain;

    using Domain.Extensions;
    using Domain.ParametersVersioning;

    using DomainModelServices;

    using DomainService.RealityObjectAccount;

    using Entities;
    using Entities.PersonalAccount;

    using Gkh.Enums.Decisions;

    using NHibernate;

    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Обработчик расчёта ЛС
    /// </summary>
    public class PersonalAccountChargeExecutor : ITaskExecutor
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        private IPerformanceLogger performanceLogger;
        private string calcGuid;

        /// <summary>
        /// Метод выполнить
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <param name="ctx">Контекст выполнения</param>
        /// <param name="indicator">Индикатор прогресса</param>
        /// <param name="ct">Обработчик отмены</param>
        /// <returns>Результат выполнения</returns>
        public IDataResult Execute(BaseParams baseParams, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var accountIds = baseParams.Params.GetAs<long[]>("idsPortion");

            var packetId = baseParams.Params.GetAs<long>("packetId", ignoreCase: true);
            var period = PersonalAccountChargeExecutor.GetPeriod(baseParams.Params);

            var container = ApplicationContext.Current.Container;
            var factory = container.Resolve<IPerformanceLoggerFactory>();

            this.performanceLogger = factory.GetLogger(this.calcGuid = baseParams.Params.GetAs("calcGuid", string.Empty));
            var viewAccountDomain = container.ResolveDomain<ViewPersonalAccount>();
            var persAccountDomain = container.ResolveDomain<BasePersonalAccount>();
            var filterService = container.Resolve<IPersonalAccountFilterService>();
            var logger = container.Resolve<ILogger>();
            var sessionProvider = container.Resolve<ISessionProvider>();

            using (container.Using(viewAccountDomain, filterService, logger, sessionProvider, factory))
            {
                var session = sessionProvider.CreateNewSession();

                session.FlushMode = FlushMode.Never;

                var persAccQuery = persAccountDomain.GetAll().ToDto()
                    .WhereIf(accountIds != null && accountIds.Length > 0, x => accountIds.Contains(x.Id));

                var accountCount = persAccQuery.Count();

                var portion = persAccountDomain.GetAll()
                    .Where(x => persAccQuery.Any(y => y.Id == x.Id));

                try
                {
                    this.performanceLogger.StartTimer("ProcessPortion", "Обработка порции ЛС");
                    this.ProcessPortion(portion, period, packetId, ct);
                    this.performanceLogger.StopTimer("ProcessPortion");
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                    throw;
                }

                indicator.Indicate(null, 90, $"Обработано {accountCount}");

                logger.LogDebug($"Обработано {accountCount}");
                sessionProvider.CloseCurrentSession();

                return new BaseDataResult(true, $"Обработано {accountCount}");
            }
        }

        private static IPeriod GetPeriod(DynamicDictionary @params)
        {
            var periodId = @params.GetAs<long>("periodId", ignoreCase: true);

            var repo = ApplicationContext.Current.Container.ResolveRepository<ChargePeriod>();
            using (ApplicationContext.Current.Container.Using(repo))
            {
                return repo.Get(periodId);
            }
        }

        private void ProcessPortion(
            IQueryable<BasePersonalAccount> portion,
            IPeriod period,
            long packetId,
            CancellationToken ct)
        {
            var container = ApplicationContext.Current.Container;
            var globalCache = container.Resolve<ICalculationGlobalCache>(new Arguments
            {
                {"performanceLogger", performanceLogger}
            });
            var packetDomain = container.ResolveDomain<UnacceptedChargePacket>();
            var factory = container.Resolve<IChargeCalculationImplFactory>();
            var roDecisionsService = container.Resolve<IRealityObjectDecisionsService>();
            var bankAccountDataProvider = container.Resolve<IBankAccountDataProvider>();
            var performedWorkDetailService = container.Resolve<IPerformedWorkDistribution>() as IPerformedWorkDetailService;
            var calcIsNotActive = container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.DisplayAfterCalculation.CalcIsNotActive;

            using (container.Using(globalCache, packetDomain, factory, performedWorkDetailService))
            {
                var packet = packetDomain.Load(packetId);

                this.performanceLogger.StartTimer("globalCache.Initialize", "Инициализация кэша");

                var sessionProvider = container.Resolve<ISessionProvider>();
                using (container.Using(sessionProvider))
                {
                    var session = sessionProvider.GetCurrentSession();
                    var conn = sessionProvider.OpenStatelessSession().Connection;

                    var hql =
                        "from BasePersonalAccount b " +
                            " left join fetch b.DecisionTariffWallet " +
                            " left join fetch b.PenaltyWallet " +
                            " left join fetch b.BaseTariffWallet " +
                            " left join fetch b.Room r " +
                            " left join fetch r.RealityObject " +
                            " where b.Id in (:ids)";
                    this.performanceLogger.StartTimer("globalCache.Initialize.PortionAccount", "Инициализация кэша по ЛС через HQL");
                    var portionAccount = session.CreateQuery(hql)
                        .SetParameterList("ids", portion.Select(x => x.Id))
                        .List<BasePersonalAccount>().ToArray();
                    this.performanceLogger.StopTimer("globalCache.Initialize.PortionAccount");
                    globalCache.Initialize(period, portion);
                    this.performanceLogger.StopTimer("globalCache.Initialize");

                    var unacceptedList = new List<UnacceptedCharge>(portionAccount.Length);

                    this.performanceLogger.StartTimer("globalCache.Initialize.AddInfo", "Инициализация кэша этап 1");
                    var ros = portionAccount.Select(pa => pa.Room.RealityObject).Distinct().ToList();
                    var rosDecisions = roDecisionsService.GetRobjectsFundFormation(ros.Select(r => r.Id).ToList());
                    var bankAccounts = bankAccountDataProvider.GetBankNumbersForCollection(ros);
                    this.performanceLogger.StopTimer("globalCache.Initialize.AddInfo");
                    this.performanceLogger.StartTimer("account.CreateChargeAll", "Расчёт всех ЛС");
                    foreach (var account in portionAccount)
                    {
                        ct.ThrowIfCancellationRequested();
                        var charge = account.CreateCharge(period, packet, factory);

                        charge.CrFundFormationDecisionType = CrFundFormationDecisionType.Unknown;
                        if (rosDecisions.ContainsKey(account.Room.RealityObject.Id))
                        {
                            charge.CrFundFormationDecisionType = rosDecisions[account.Room.RealityObject.Id].First().Item2;
                        }

                        string accountNumber;
                        bankAccounts.TryGetValue(account.Room.RealityObject.Id, out accountNumber);
                        charge.ContragentAccountNumber = accountNumber;

                        unacceptedList.Add(charge);
                    }
                    this.performanceLogger.StopTimer("account.CreateChargeAll");

                    this.performanceLogger.StartTimer("DapperSave", "Сохранение всех данных");
                    var trans = conn.BeginTransaction();
                    var portionAccountIds = portionAccount.Select(x => x.Id).ToList();
                    try
                    {
                        unacceptedList.ForEach(x =>
                        {
                            x.ObjectCreateDate = DateTime.Now;
                            x.ObjectEditDate = DateTime.Now;
                            x.CalculatedParams.ForEach(
                                a =>
                                {
                                    a.ObjectCreateDate = DateTime.Now;
                                    a.ObjectEditDate = DateTime.Now;
                                });
                            x.ParameterTraces.ForEach(
                                b =>
                                {
                                    b.ObjectCreateDate = DateTime.Now;
                                    b.ObjectEditDate = DateTime.Now;
                                });
                            x.RecalcHistory.ForEach(
                                c =>
                                {
                                    c.ObjectCreateDate = DateTime.Now;
                                    c.ObjectEditDate = DateTime.Now;
                                });
                        });

                        string sql;

                        if (!calcIsNotActive)
                        {
                            // Удаляем существующие истории перерасчета
                            sql =
                                $@" DELETE FROM public.regop_recalc_history
                                  WHERE calc_period_id={period.Id} 
                                   AND account_id IN ({string
                                    .Join(",", portionAccountIds)})";
                            conn.Execute(sql, transaction: trans);
                        }

                        if (!calcIsNotActive)
                        {
                            // обновляем подтверждённые начисления
                            sql = $@" UPDATE public.regop_pers_acc_charge_period_{period.Id} SET is_active = false, object_edit_date = current_timestamp
                                  WHERE pers_acc_id IN ({string.Join(",", portionAccountIds)}) and is_active";
                            conn.Execute(sql, transaction: trans);
                        }

                        const int bulkSize = 5000;
                        sql =
                            $@"INSERT INTO public.regop_pers_acc_charge_period_{period.Id} (object_version, object_create_date, object_edit_date,
                                           charge_date, guid, charge, charge_tariff, penalty, recalc, pers_acc_id,
                                           overplus, is_fixed, is_active, recalc_decision, recalc_penalty, period_id, packet_id) 
                                   VALUES (@ObjectVersion, @ObjectCreateDate, @ObjectEditDate,@ChargeDate,@Guid,@Charge,@ChargeTariff,
                                           @Penalty,@RecalcByBaseTariff,@BasePersonalAccountId,@OverPlus,@IsFixed, @IsActive, @RecalcByDecisionTariff,
                                           @RecalcPenalty,@ChargePeriodId, @PacketId)";
                        var personalAccountCharges = unacceptedList.Select(x => new PersonalAccountChargeDto(x, period, calcIsNotActive)).ToList();
                        conn.Execute(sql, personalAccountCharges.ToArray(), trans);

                        sql =
                            $@"INSERT INTO public.regop_persaccalcparamtmp (object_version, object_create_date, object_edit_date, logged_entity_id, pers_acc_id)
                                  VALUES (@ObjectVersion, @ObjectCreateDate, @ObjectEditDate, @LoggedEntityId, @PersonalAccountId);";
                        var calcParamsDtoList = unacceptedList.SelectMany(x => x.CalculatedParams).Select(x => new PersonalAccountCalcParamDto(x));
                        conn.Execute(sql, calcParamsDtoList.ToArray(), trans);

                        sql =
                            @"INSERT INTO public.regop_calc_param_trace_period_{0} (object_version, object_create_date, object_edit_date, calc_guid, calc_type,
                                          date_start, param_values, date_end, period_id)
                                    VALUES ";
                        var paramTracesDtoList = unacceptedList.SelectMany(x => x.ParameterTraces).Select(x => new CalculationParameterTraceDto(x)).ToList();

                        foreach (var periodId in paramTracesDtoList.Select(x => x.ChargePeriodId).Distinct())
                        {
                            var collectionToInsert = paramTracesDtoList.Where(x => x.ChargePeriodId == periodId).ToArray();
                            var bulks = collectionToInsert.SplitArray(bulkSize).ToList();
                            foreach (var bulk in bulks)
                            {
                                var insertQuery = string.Format(sql, periodId);
                                insertQuery = string.Concat(insertQuery, string.Join(",", bulk.Select(x =>
                                         $@"({x.ObjectVersion}, '{x.ObjectCreateDate}', '{x.ObjectEditDate}', '{x.CalculationGuid}',
                                         {x.CalculationTypeId}, '{x.DateStart}', '{x.ParameterValuesString}', '{x.DateEnd}', {x.ChargePeriodId})")));
                                conn.Execute(insertQuery, transaction: trans);
                            }
                        }

                        sql =
                            @"INSERT INTO public.regop_recalc_history_period_{0} ( object_version, object_create_date, object_edit_date, account_id, 
                                         calc_period_id, recalc_period_id, recalc_sum, unaccepted_guid, recalc_type)
                                 VALUES (@ObjectVersion, @ObjectCreateDate, @ObjectEditDate, @PersonalAccountId, @CalcPeriodId, 
                                         @RecalcPeriodId, @RecalcSum, @UnacceptedChargeGuid, @RecalcTypeId);";
                        var recalcHistotyDtoList = unacceptedList.SelectMany(x => x.RecalcHistory).Select(x => new RecalcHistoryDto(x));
                        foreach (var periodId in recalcHistotyDtoList.Select(x => x.RecalcPeriodId).Distinct())
                        {
                            conn.Execute(string.Format(sql, periodId), recalcHistotyDtoList.Where(x => x.RecalcPeriodId == periodId).ToArray(), trans);
                        }

                        if (!calcIsNotActive)
                        {
                            //получаем сальдовые строки за текущий период
                            sql =
                                $@" SELECT s.Id as Id, s.ACCOUNT_ID as PersonalAccountId, s.PERIOD_ID as PeriodId, 0 as ChargeTariff,
                                 0 as ChargedByBaseTariff, 0 as RecalcByBaseTariff, 0 as RecalcByDecisionTariff,
                                 0 as RecalcByPenalty,
                                 0 as Penalty, s.PENALTY_PAYMENT as PenaltyPayment, s.TARIFF_PAYMENT as TariffPayment,
                                 s.SALDO_IN as SaldoIn, 0 as SaldoOut, s.TARIFF_DESICION_PAYMENT as TariffDecisionPayment, 
                                 s.OVERHAUL_PAYMENT as OverhaulPayment, s.RECRUITMENT_PAYMENT as RecruitmentPayment, 
                                 s.SALDO_IN_SERV as SaldoInFromServ, s.SALDO_OUT_SERV as SaldoOutFromServ, s.SALDO_CHANGE_SERV as SaldoChangeFromServ, 
                                 s.BALANCE_CHANGE as BaseTariffChange, s.DEC_BALANCE_CHANGE as DecisionTariffChange, s.PENALTY_BALANCE_CHANGE as PenaltyChange,
                                 s.BASE_TARIFF_DEBT as BaseTariffDebt, s.DEC_TARIFF_DEBT as DecisionTariffDebt, s.PENALTY_DEBT as PenaltyDebt, 
                                 s.PERF_WORK_CHARGE as PerformedWorkCharged
                                FROM public.REGOP_PERS_ACC_PERIOD_SUMM_PERIOD_{period
                                    .Id} s 
                                WHERE s.ACCOUNT_ID IN ({string.Join(",", portionAccountIds)})";
                            try
                            {
                                var accountSummaries =
                                    conn.Query<PersonalAccountPeriodSummaryDto>(sql, transaction: trans).ToDictionary(x => x.PersonalAccountId);

                                this.performanceLogger.StartTimer("GetResultDistributionSum", "Получение планируемых распределений");
                                IDictionary<long, Tuple<decimal, decimal>> supposedPerfWorkCharges = null;
                                container.InTransactionInNewScope(
                                    () => supposedPerfWorkCharges = performedWorkDetailService?.GetResultDistributionSum(portion, conn));
                                supposedPerfWorkCharges = supposedPerfWorkCharges ?? new Dictionary<long, Tuple<decimal, decimal>>();
                                this.performanceLogger.StopTimer("GetResultDistributionSum");

                                var accountDict = portionAccount.ToDictionary(x => x.Id);

                                //пересчитывем поля, которые зависят от расчета
                                foreach (var charge in personalAccountCharges)
                                {
                                    if (!accountSummaries.ContainsKey(charge.BasePersonalAccountId))
                                    {
                                        throw new KeyNotFoundException(
                                            $"Не найден regop_pers_acc_period_summ для ЛС с ID = {charge.BasePersonalAccountId}");
                                    }

                                    //применяем начисления
                                    var accountSumm = accountSummaries[charge.BasePersonalAccountId];
                                    accountSumm.ApplyCharge(charge);
                                    var walletGuids = globalCache.GetWalletByAccountId(accountSumm.PersonalAccountId);

                                    //Расчет + Отмены + Слияние
                                    accountSumm.ChargeTariff +=
                                        globalCache.GetCancelChargesInCurrentPeriod(accountSumm.PersonalAccountId, CancelType.BaseTariffCharge)
                                            .SafeSum(x => -x.CancelSum) +
                                            globalCache.GetCancelChargesInCurrentPeriod(accountSumm.PersonalAccountId, CancelType.DecisionTariffCharge)
                                                .SafeSum(x => -x.CancelSum) +
                                            globalCache.GetTransferFromAccountsInCurrentPeriod(walletGuids.BaseTariffWalletGuid).SafeSum(x => x.Amount) +
                                            globalCache.GetTransferFromAccountsInCurrentPeriod(walletGuids.DecisionTariffWalletGuid).SafeSum(x => x.Amount);

                                    accountSumm.ChargedByBaseTariff += globalCache
                                            .GetCancelChargesInCurrentPeriod(accountSumm.PersonalAccountId, CancelType.BaseTariffCharge)
                                            .SafeSum(x => -x.CancelSum) +
                                        globalCache.GetTransferFromAccountsInCurrentPeriod(walletGuids.BaseTariffWalletGuid).SafeSum(x => x.Amount);

                                    //Расчет + Отмены + Слияние
                                    accountSumm.Penalty +=
                                        globalCache.GetCancelChargesInCurrentPeriod(accountSumm.PersonalAccountId, CancelType.Penalty)
                                            .SafeSum(x => -x.CancelSum) +
                                            globalCache.GetTransferFromAccountsInCurrentPeriod(walletGuids.PenaltyWalletGuid).SafeSum(x => x.Amount);

                                    // Старый зачёт + Новый + Предполагаемый
                                    accountSumm.PerformedWorkCharged =
                                        supposedPerfWorkCharges
                                            .Get<long, Tuple<decimal, decimal>>(accountSumm.PersonalAccountId, Tuple.Create(0m, 0m)).Item1

                                            + globalCache.GetPerfWorkCharge(accountDict.Get(accountSumm.PersonalAccountId))
                                                .Where(x => x.ChargePeriod.Id == charge.ChargePeriodId)
                                                .SafeSum(x => x.Sum)
                                            + globalCache.GetPerfWorks(accountDict.Get(accountSumm.PersonalAccountId))
                                                .Where(x => x.ChargePeriodId == charge.ChargePeriodId)
                                                .SafeSum(x => x.Amount);

                                    accountSumm.DecsicionPerformedWorkCharged = supposedPerfWorkCharges
                                        .Get<long, Tuple<decimal, decimal>>(accountSumm.PersonalAccountId, Tuple.Create(0m, 0m)).Item2;

                                    //пересчитываем сальдо
                                    accountSumm.RecalcSaldoOut();

                                    sql =
                                        $@"UPDATE public.regop_pers_acc_period_summ_period_{period.Id} 
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
                                    conn.Execute(sql, accountSumm, trans);
                                }
                            }
                            catch (ArgumentException exception)
                            {
                                throw new Exception($"Обнаружены дубли в regop_pers_acc_period_summ!", exception.InnerException);
                            }
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    trans.Commit();
                    this.performanceLogger.StopTimer("DapperSave");

                    // чистим кэш (т.к. нам он ещё нужен для пересчёта показателей)
                    globalCache.Clear();
                }
            }
        }

    }
}