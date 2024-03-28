namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Contracts.Params;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using NHibernate;

    /// <summary>
    /// Сервис массовых отмен начислений ЛС
    /// </summary>
    public class PersonalAccountCancelChargeService : IPersonalAccountCancelChargeService
    {
        private readonly IPersonalAccountHistoryCreator historyCreator;

        private readonly IRealtyObjectPaymentSession realtyObjectPaymentSession;

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Интерфейс менеджера пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="MoneyOperation"/>
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="CancelChargeSource"/>
        /// </summary>
        public IDomainService<CancelChargeSource> CancelChargeSourceDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="CancelCharge"/>
        /// </summary>
        public IDomainService<CancelCharge> CancelChargeDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="SaldoChangeDetail"/>
        /// </summary>
        public IDomainService<SaldoChangeDetail> SaldoChangeDetailDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Transfer"/>
        /// </summary>
        public ITransferDomainService TransferDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Transfer"/>
        /// </summary>
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountPeriodSummary"/>
        /// </summary>
        public IDomainService<PersonalAccountCharge> PersonalAccountChargeDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountChange"/>
        /// </summary>
        public IDomainService<PersonalAccountChange> PersonalAccountChangeDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="BasePersonalAccount"/>
        /// </summary>
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        /// <summary>
        /// Репозиторий <see cref="BasePersonalAccount"/>
        /// </summary>
        public IRepository<BasePersonalAccount> BasePersonalAccountRepository { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountPeriodSummary"/>
        /// </summary>
        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RecalcHistory"/>
        /// </summary>
        public IDomainService<RecalcHistory> RecalcHistoryDomain { get; set; }

        /// <summary>
        /// Сервис фильтрации ЛС
        /// </summary>
        public IPersonalAccountFilterService AccountFilterService { get; set; }

        /// <summary>
        /// Репозиторий периодов начисления
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <summary>
        /// Провайдер сессий
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Менеджер запрета перерасчета
        /// </summary>
        public IPersonalAccountBanRecalcManager BanRecalcManager { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="historyCreator">Создатель истории изменения</param>
        /// <param name="realtyObjectPaymentSession">Сессия оплат дома</param>
        public PersonalAccountCancelChargeService(IPersonalAccountHistoryCreator historyCreator, IRealtyObjectPaymentSession realtyObjectPaymentSession)
        {
            this.historyCreator = historyCreator;
            this.realtyObjectPaymentSession = realtyObjectPaymentSession;
        }

        /// <summary>
        /// Произвести отмену начислений
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult CancelCharges(BaseParams baseParams)
        {
            return this.Container.InTransactionWithResult(() => this.CancelChargesInternal(baseParams));
        }

        /// <summary>
        /// Получить данные для отображения
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns></returns>
        public IDataResult GetDataForUI(BaseParams baseParams)
        {
            try
            {
                var data = this.Container.InTransactionWithResult(() =>
                {
                    var currentSession = this.SessionProvider.GetCurrentSession();
                    var oldFlushMode = currentSession.FlushMode;
                    currentSession.FlushMode = FlushMode.Never;

                    try
                    {
                        return this.GetDataInternal(baseParams, true);
                    }
                    finally
                    {
                        currentSession.FlushMode = oldFlushMode;
                        currentSession.Clear();
                    }
                });
                return new ListDataResult(data.Data, data.TotalCount);
            }
            catch (ValidationException)
            {
                return new ListDataResult();
            }
        }

        private GenericListResult<PersonalAccountCancelChargeInfo> GetDataInternal(BaseParams baseParams, bool usePaging)
        {
            var loadParam = baseParams.GetLoadParam();

            var bsParams = new BaseParams();
            bsParams.Params.Set("complexFilter", baseParams.Params.Get("complexFilterGrid"));
            var loadBsParam = bsParams.GetLoadParam();

            var periodIds = baseParams.Params.GetAs<long[]>("periodIds") ?? new long[0];
            var persAccIds = baseParams.Params.GetAs<long[]>("persAccIds");
            var cancelParams = baseParams.Params.Read<CancelParams>().Execute();

            if (!cancelParams.HasAnyFilter)
            {
                throw new ValidationException("Ошибка при получении данных, выберите хотя бы один тип отмены");
            }

            var persAccQuery = this.BasePersonalAccountDomain.GetAll().ToDtoWithWallets();

            persAccQuery = persAccIds.IsNotEmpty()
                ? persAccQuery.Where(x => persAccIds.Contains(x.Id))
                : persAccQuery
                    .FilterByBaseParams(baseParams, this.Container) // Фильтруем по базовым параметрам
                    .Filter(loadBsParam, this.Container); // Сначала фильтруем по complexFilter реестра ЛС

            var persAccCount = 0;
            var accountDict = new Dictionary<long, BasePersonalAccount>();
            var currentPeriod = this.ChargePeriodRepository.GetCurrentPeriod();


            // здесь мы тянем данные либо для грида, либо для непосредственной отмены
            if (usePaging)
            {
                persAccQuery = persAccQuery.Filter(loadParam, this.Container);

                // на выходе получаем всего 25 ЛС (сколько отображается на странице)
                // и фильтр грида не влияет на пачку отменяемых
                persAccCount = persAccQuery.Count();
                persAccQuery = persAccQuery
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address)
                    .Order(loadParam)
                    .Paging(loadParam);
            }
            else
            {
                // для непосредственной отмены грузим все ЛС
                if (persAccIds.IsNotEmpty())
                {
                    var periodSummaryDict = this.PersonalAccountPeriodSummaryDomain.GetAll()
                            .WhereContains(x => x.PersonalAccount.Id, persAccIds)
                            .Where(x => x.Period == currentPeriod)
                            .ToDictionary(x => x.PersonalAccount);

                    accountDict = this.BasePersonalAccountDomain.GetAll()
                        .FetchAllWallets()
                        .WhereContains(x => x.Id, persAccIds)
                        .ToDictionary(x => x.Id);

                    foreach (var account in accountDict)
                    {
                        account.Value.SetOpenedPeriodSummary(periodSummaryDict.Get(account.Value));
                    }
                }
                else
                {
                    var accountIds = persAccQuery.Select(x => x.Id).ToArray();
                    foreach (var accIds in accountIds.Section(5000))
                    {
                        var periodSummaryDict = this.PersonalAccountPeriodSummaryDomain.GetAll()
                            .WhereContains(x => x.PersonalAccount.Id, accIds)
                            .Where(x => x.Period == currentPeriod)
                            .ToDictionary(x => x.PersonalAccount);

                        var data = this.BasePersonalAccountDomain.GetAll()
                            .FetchAllWallets()
                            .WhereContains(x => x.Id, accIds)
                            .ToArray();

                        foreach (var account in data)
                        {
                            accountDict[account.Id] = account;
                            account.SetOpenedPeriodSummary(periodSummaryDict.Get(account));
                        }
                    }
                }
            }

            // кошельки ЛС для трансферов слияния
            var walletGuidsByPersAccData = persAccQuery.Select(
                    x => new
                    {
                        x.Id,
                        x.PersonalAccountNum,
                        x.RoomAddress,
                        x.Municipality,

                        BaseTariffWallet = x.BaseTariffWalletGuid,
                        DecisionTariffWallet = x.DecisionTariffWalletGuid,
                        PenaltyWallet = x.PenaltyWalletGuid,
                    })
                .AsEnumerable()
                .ToDictionary(x => x.Id);

            // периоды начислений
            var chargePeriodQuery = this.ChargePeriodDomain.GetAll()
                .Where(x => x.IsClosed)
                .WhereIf(periodIds.Length > 0, x => periodIds.Contains(x.Id));

            var chargePeriodDict = chargePeriodQuery.ToDictionary(x => x.Id);
            var chargePeriodIds = chargePeriodDict.Keys.ToArray();

            var resultList = new List<PersonalAccountCancelChargeInfo>();
            foreach (var portion in walletGuidsByPersAccData.Section(1000))
            {
                var walletGuidsByPersAcc = portion.ToList();
                var accIdsFiltered = walletGuidsByPersAcc.Select(x => x.Key).ToList();

                // предыдущие отмены 
                var cancelChargesDict = this.CancelChargeDomain.GetAll()
                    .Where(x => accIdsFiltered.Contains(x.PersonalAccount.Id))
                    .WhereIf(chargePeriodIds.Length > 0, x => chargePeriodIds.Contains(x.CancelPeriod.Id))
                    .WhereIf(!cancelParams.CancelBaseTariffCharge, x => x.CancelType != CancelType.BaseTariffCharge)
                    .WhereIf(!cancelParams.CancelBaseTariffChange, x => x.CancelType != CancelType.BaseTariffChange)
                    .WhereIf(!cancelParams.CancelDecisionTariffCharge, x => x.CancelType != CancelType.DecisionTariffCharge)
                    .WhereIf(!cancelParams.CancelDecisionTariffChange, x => x.CancelType != CancelType.DecisionTariffChange)
                    .WhereIf(!cancelParams.CancelPenaltyCharge, x => x.CancelType != CancelType.Penalty)
                    .WhereIf(!cancelParams.CancelPenaltyChange, x => x.CancelType != CancelType.PenaltyChange)
                    .Select(x => new
                    {
                        AccountId = x.PersonalAccount.Id,
                        x.CancelPeriod,
                        x.CancelType,
                        x.CancelSum
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.AccountId)
                    .ToDictionary(
                        x => x.Key,
                        x => x.GroupBy(y => y.CancelPeriod).ToDictionary(
                            y => y.Key,
                            y => y.GroupBy(z => z.CancelType).ToDictionary(z => z.Key, s => s.Sum(z => z.CancelSum.RegopRoundDecimal(2)))));
                
                var saldoChangeDict = new Dictionary<long, Dictionary<ChargePeriod, Dictionary<WalletType, decimal>>>();
                var mergeInfoSourceDict = new Dictionary<long, Dictionary<ChargePeriod, BalanceInfo>>();
                var mergeInfoTargetDict = new Dictionary<long, Dictionary<ChargePeriod, BalanceInfo>>();
                var recalcHistoryDict = new Dictionary<long, Dictionary<ChargePeriod, BalanceInfo>>();

                // ручные корректировки
                if (cancelParams.HasAnyChangeFilter)
                {
                    saldoChangeDict = this.SaldoChangeDetailDomain.GetAll()
                        .Where(x => accIdsFiltered.Contains(x.PersonalAccount.Id))
                        .WhereIf(chargePeriodIds.Length > 0, x => chargePeriodIds.Contains(x.ChargeOperation.Period.Id))
                        .WhereIf(!cancelParams.CancelBaseTariffChange, x => x.ChangeType != WalletType.BaseTariffWallet)
                        .WhereIf(!cancelParams.CancelDecisionTariffChange, x => x.ChangeType != WalletType.DecisionTariffWallet)
                        .WhereIf(!cancelParams.CancelPenaltyChange, x => x.ChangeType != WalletType.PenaltyWallet)
                        .Select(x => new
                        {
                            AccountId = x.PersonalAccount.Id,
                            x.ChargeOperation.Period,
                            x.ChangeType,
                            Amount = x.NewValue - x.OldValue
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.AccountId)
                        .ToDictionary(
                            x => x.Key,
                            x => x.GroupBy(z => z.Period)
                                .ToDictionary(
                                    z => z.Key,
                                    s => s.GroupBy(z => z.ChangeType).ToDictionary(z => z.Key, q => q.Sum(z => z.Amount))));
                }

                // если нужно отменять начисление, только тогда получаем
                if (cancelParams.HasAnyChargeFilter)
                {
                    // трансферы слияния
                    var walletGuids = walletGuidsByPersAcc.Select(x => x.Value)
                        .SelectMany(x =>
                            new[]
                            {
                                (cancelParams.CancelBaseTariffCharge ? x.BaseTariffWallet : null),
                                (cancelParams.CancelDecisionTariffCharge ? x.DecisionTariffWallet : null),
                                (cancelParams.CancelPenaltyCharge ? x.PenaltyWallet : null)
                            })
                        .Where(x => x != null)
                        .ToArray();

                    var mergeTransfersSourceDict = this.TransferDomain.GetAll<PersonalAccountChargeTransfer>()
                        .Where(x => accIdsFiltered.Contains(x.Owner.Id))
                        .Where(x => walletGuids.Contains(x.SourceGuid))
                        .Where(x => x.Reason == null) // Перенос долга при слиянии
                        .Where(x => chargePeriodIds.Contains(x.ChargePeriod.Id))
                        .Where(x => x.Amount > 0)
                        .Select(x => new
                        {
                            x.SourceGuid,
                            x.ChargePeriod,
                            x.Amount
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.SourceGuid)
                        .ToDictionary(
                            x => x.Key,
                            x => x.GroupBy(y => y.ChargePeriod)
                                .ToDictionary(
                                    y => y.Key,
                                y => y.SafeSum(z => z.Amount)));

                    mergeInfoSourceDict = chargePeriodDict.Values
                        .SelectMany(z =>
                            walletGuidsByPersAcc.Select(x =>
                                new
                                {
                                    Id = x.Key,
                                    Period = z,
                                    BaseTariff = mergeTransfersSourceDict.Get(x.Value.BaseTariffWallet)?.Get(z) ?? 0,
                                    DecisionTariff = mergeTransfersSourceDict.Get(x.Value.DecisionTariffWallet)?.Get(z) ?? 0,
                                    Penalty = mergeTransfersSourceDict.Get(x.Value.PenaltyWallet)?.Get(z) ?? 0
                                }))
                        .GroupBy(x => x.Id)
                        .ToDictionary(
                            x => x.Key,
                            x => x.GroupBy(y => y.Period)
                                .ToDictionary(
                                    y => y.Key,
                                    y => new BalanceInfo
                                    {
                                        BaseTariff = y.Sum(z => z.BaseTariff),
                                        DecisionTariff = y.Sum(z => z.DecisionTariff),
                                        Penalty = y.Sum(z => z.Penalty)
                                    }));

                    var mergeTargetTransfersDict = this.TransferDomain.GetAll<PersonalAccountChargeTransfer>()
                       .Where(x => accIdsFiltered.Contains(x.Owner.Id))
                       .Where(x => walletGuids.Contains(x.TargetGuid))
                       .Where(x => x.Reason == null) // Перенос долга при слиянии
                       .Where(x => chargePeriodIds.Contains(x.ChargePeriod.Id))
                       .Where(x => x.Amount > 0)
                       .Select(x => new
                       {
                           x.TargetGuid,
                           x.ChargePeriod,
                           x.Amount
                       })
                       .AsEnumerable()
                       .GroupBy(x => x.TargetGuid)
                       .ToDictionary(
                           x => x.Key,
                           x => x.GroupBy(y => y.ChargePeriod)
                               .ToDictionary(
                                   y => y.Key,
                               y => y.SafeSum(z => z.Amount)));

                    mergeInfoTargetDict = chargePeriodDict.Values
                       .SelectMany(z =>
                           walletGuidsByPersAcc.Select(x =>
                               new
                               {
                                   Id = x.Key,
                                   Period = z,
                                   BaseTariff = mergeTargetTransfersDict.Get(x.Value.BaseTariffWallet)?.Get(z) ?? 0,
                                   DecisionTariff = mergeTargetTransfersDict.Get(x.Value.DecisionTariffWallet)?.Get(z) ?? 0,
                                   Penalty = mergeTargetTransfersDict.Get(x.Value.PenaltyWallet)?.Get(z) ?? 0
                               }))
                       .GroupBy(x => x.Id)
                       .ToDictionary(
                           x => x.Key,
                           x => x.GroupBy(y => y.Period)
                               .ToDictionary(
                                   y => y.Key,
                                   y => new BalanceInfo
                                   {
                                       BaseTariff = y.Sum(z => z.BaseTariff),
                                       DecisionTariff = y.Sum(z => z.DecisionTariff),
                                       Penalty = y.Sum(z => z.Penalty)
                                   }));

                    // перерасчёты, чтобы учесть фактическое начисление за период
                    recalcHistoryDict = this.RecalcHistoryDomain.GetAll()
                        .Where(x => x.CalcPeriod.Id != currentPeriod.Id)           // не берем recalc history текущего периода
                        .Where(x => accIdsFiltered.Contains(x.PersonalAccount.Id))
                        .Where(x => chargePeriodIds.Contains(x.RecalcPeriod.Id))
                        .Where(x => this.PersonalAccountChargeDomain.GetAll().Any(y => y.Guid == x.UnacceptedChargeGuid && y.BasePersonalAccount == x.PersonalAccount))
                        .WhereIf(!cancelParams.CancelBaseTariffCharge, x => x.RecalcType != (RecalcType)CancelType.BaseTariffCharge)
                        .WhereIf(!cancelParams.CancelDecisionTariffCharge, x => x.RecalcType != (RecalcType)CancelType.DecisionTariffCharge)
                        .WhereIf(!cancelParams.CancelPenaltyCharge, x => x.RecalcType != (RecalcType)CancelType.Penalty)
                        .Select(x => new
                        {
                            AccountId = x.PersonalAccount.Id,
                            x.RecalcPeriod,
                            x.RecalcType,
                            x.RecalcSum
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.AccountId)
                        .ToDictionary(
                            x => x.Key,
                            y => y.GroupBy(x => x.RecalcPeriod)
                                .ToDictionary(
                                    x => x.Key,
                                    x => new BalanceInfo
                                    {
                                        BaseTariff = x.Where(z => z.RecalcType == RecalcType.BaseTariffCharge).Sum(z => z.RecalcSum),
                                        DecisionTariff = x.Where(z => z.RecalcType == RecalcType.DecisionTariffCharge).Sum(z => z.RecalcSum),
                                        Penalty = x.Where(z => z.RecalcType == RecalcType.Penalty).Sum(z => z.RecalcSum)
                                    }));
                }

                var chargeDict = this.PersonalAccountChargeDomain.GetAll()
                    .Where(x => x.IsActive)
                    .Where(x => accIdsFiltered.Contains(x.BasePersonalAccount.Id))
                    .Where(x => x.IsFixed)
                    .Where(x => chargePeriodIds.Contains(x.ChargePeriod.Id))
                    .Select(x => new
                    {
                        AccountId = x.BasePersonalAccount.Id,
                        PeriodId = x.ChargePeriod.Id,
                        x.ChargeTariff,
                        x.OverPlus,
                        x.Penalty
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.AccountId)
                    .ToDictionary(
                        x => x.Key,
                        y => y.ToDictionary(x => chargePeriodDict.Get(x.PeriodId)));

                     var cancelChargeInfos = walletGuidsByPersAcc.SelectMany(
                    account => chargePeriodDict.Values
                        .Select(x =>
                        {
                            var charges = chargeDict.Get(account.Key)?.Get(x);
                            var acc = account.Value;
                            return new PersonalAccountCancelChargeInfo
                            {
                                Id = account.Key,
                                Account = accountDict.Get(account.Key),
                                Period = x,
                                Municipality = acc.Municipality,
                                RoomAddress = acc.RoomAddress,
                                PersonalAccountNum = acc.PersonalAccountNum,

                          BaseTariffSum =
                                    ((cancelParams.CancelBaseTariffCharge ? charges.ReturnSafe(y => y.ChargeTariff - y.OverPlus) : 0)
                                    + (recalcHistoryDict.Get(acc.Id)?.Get(x)?.BaseTariff ?? 0))
                                    - ((cancelChargesDict.Get(acc.Id)?.Get(x)?.Get(CancelType.BaseTariffCharge) ?? 0)
                                    + (mergeInfoSourceDict.Get(acc.Id)?.Get(x)?.BaseTariff ?? 0)
                                    - (mergeInfoTargetDict.Get(acc.Id)?.Get(x)?.BaseTariff ?? 0)),

                                DecisionTariffSum =
                                    ((cancelParams.CancelDecisionTariffCharge ? charges.ReturnSafe(y => y.OverPlus) : 0)
                                    + (recalcHistoryDict.Get(acc.Id)?.Get(x)?.DecisionTariff ?? 0))
                                    - ((cancelChargesDict.Get(acc.Id)?.Get(x)?.Get(CancelType.DecisionTariffCharge) ?? 0)
                                    + (mergeInfoSourceDict.Get(acc.Id)?.Get(x)?.DecisionTariff ?? 0)
                                    - (mergeInfoTargetDict.Get(acc.Id)?.Get(x)?.DecisionTariff ?? 0)),
                                PenaltySum =
                                    ((cancelParams.CancelPenaltyCharge ? charges.ReturnSafe(y => y.Penalty) : 0)
                                    + (recalcHistoryDict.Get(acc.Id)?.Get(x)?.Penalty ?? 0))
                                    - ((cancelChargesDict.Get(acc.Id)?.Get(x)?.Get(CancelType.Penalty) ?? 0)
                                    + (mergeInfoSourceDict.Get(acc.Id)?.Get(x)?.Penalty ?? 0)
                                    - (mergeInfoTargetDict.Get(acc.Id)?.Get(x)?.Penalty ?? 0)),

                                BaseTariffChange = (saldoChangeDict.Get(acc.Id)?.Get(x)?.Get(WalletType.BaseTariffWallet) ?? 0)
                                    - (cancelChargesDict.Get(acc.Id)?.Get(x)?.Get(CancelType.BaseTariffChange) ?? 0),
                                DecisionTariffChange = (saldoChangeDict.Get(acc.Id)?.Get(x)?.Get(WalletType.DecisionTariffWallet) ?? 0)
                                    - (cancelChargesDict.Get(acc.Id)?.Get(x)?.Get(CancelType.DecisionTariffChange) ?? 0),
                                PenaltyChange = (saldoChangeDict.Get(acc.Id)?.Get(x)?.Get(WalletType.PenaltyWallet) ?? 0)
                                    - (cancelChargesDict.Get(acc.Id)?.Get(x)?.Get(CancelType.PenaltyChange) ?? 0)
                            };
                        }));

                resultList.AddRange(cancelChargeInfos);
            }

            var result = this.GroupByAccountIf(usePaging, resultList);
            return new GenericListResult<PersonalAccountCancelChargeInfo>(result, persAccCount);
        }

        private IEnumerable<PersonalAccountCancelChargeInfo> GroupByAccountIf(
            bool condition,
            IEnumerable<PersonalAccountCancelChargeInfo> cancelChargeInfos)
        {
            if (!condition)
            {
                return cancelChargeInfos;
            }

            // если отображаем в UI, то схлопываем по ЛС
            return cancelChargeInfos.GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => new PersonalAccountCancelChargeInfo
                    {
                        Id = x.Key,
                        Account = x.First().Account,
                        Period = x.First().Period,

                        Municipality = x.First().Municipality,
                        RoomAddress = x.First().RoomAddress,
                        PersonalAccountNum = x.First().PersonalAccountNum,

                        BaseTariffSum = x.Sum(z => z.BaseTariffSum),
                        DecisionTariffSum = x.Sum(z => z.DecisionTariffSum),
                        PenaltySum = x.Sum(z => z.PenaltySum),

                        BaseTariffChange = x.Sum(z => z.BaseTariffChange),
                        DecisionTariffChange = x.Sum(z => z.DecisionTariffChange),
                        PenaltyChange = x.Sum(z => z.PenaltyChange),
                    })
                .Values;
        }
    
        private IDataResult CancelChargesInternal(BaseParams baseParams)
        {
            var currentPeriod = this.ChargePeriodDomain.GetAll().First(x => x.IsClosed == false);

            var changeInfo = PersonalAccountChangeInfo.FromParams(baseParams);
            changeInfo.DateActual = currentPeriod.StartDate;

            // создаем общую операцию отмены
            var cancelChargeSource = new CancelChargeSource(currentPeriod)
            {
                Document = changeInfo.Document,
                Reason = changeInfo.Reason,
                User = this.UserManager.GetActiveUser()?.Login ?? "anonymous",
                OperationDate = currentPeriod.StartDate
            };

            var moneyOperation = cancelChargeSource.CreateOperation(currentPeriod);

            var transfers = new List<Transfer>();
            var changes = new List<PersonalAccountChange>();
            var summaries = new List<PersonalAccountPeriodSummary>();

            // а здесь мы тянем все все данные, очень тяжёлый запрос
            var cancelInfos = this.GetDataInternal(baseParams, false);

            try
            {
                foreach (var cancelInfo in cancelInfos.Data.ToLookup(x => x.Account))
                {
                    var account = cancelInfo.Key;

                    cancelInfo.ForEach(x =>
                    {
                        cancelChargeSource.AddCancelCharge(x);

                        // создать трансферы
                        transfers.AddRange(x.Account.UndoCharge(cancelChargeSource, moneyOperation, x, currentPeriod, changeInfo));

                        var isCharge = x.BaseTariffSum != 0 || x.DecisionTariffSum != 0 ? BanRecalcType.Charge : 0;
                        var isPenalty = x.PenaltySum != 0 ? BanRecalcType.Penalty : 0;
                        var type = isCharge | isPenalty;

                        if (Enum.IsDefined(typeof(BanRecalcType), type))
                        {
                            this.BanRecalcManager.CreateBanRecalc(
                                x.Account,
                                x.Period.StartDate,
                                x.Period.GetEndDate(),
                                type,
                                cancelChargeSource.Document,
                                "Отмена начислений и корректировок за периоды");
                        }
                    });
                    
                    // создание записи в истории
                    changes.AddRange(this.GetAccountChanges(cancelInfo.ToList(), account, currentPeriod, changeInfo));

                    summaries.Add(account.GetOpenedPeriodSummary());
                }

                this.CancelChargeSourceDomain.Save(cancelChargeSource);
                this.MoneyOperationDomain.Save(moneyOperation);

                transfers.ForEach(this.TransferDomain.Save);
                cancelChargeSource.CancelCharges.ForEach(this.CancelChargeDomain.Save);
                changes.ForEach(this.PersonalAccountChangeDomain.Save);
                summaries.ForEach(this.PersonalAccountPeriodSummaryDomain.Save);

                // необходимо в счёт начислений посадить изменения
                this.realtyObjectPaymentSession.Complete();
                this.BanRecalcManager.SaveBanRecalcs();
                return new BaseDataResult();
            }
            catch
            {
                this.realtyObjectPaymentSession.Rollback();
                throw;
            }
        }

        private List<PersonalAccountChange> GetAccountChanges(
            List<PersonalAccountCancelChargeInfo> infos,
            BasePersonalAccount account,
            ChargePeriod currentPeriod, 
            PersonalAccountChangeInfo changeInfo)
        {
            var periods = infos.Select(x => x.Period.Name).AggregateWithSeparator(",");

            var changes = new List<PersonalAccountChange>();

            var baseSum = infos.SafeSum(x => x.BaseTariffSum).RegopRoundDecimal(2);
            var decSum = infos.SafeSum(x => x.DecisionTariffSum).RegopRoundDecimal(2);
            var penaltySum = infos.SafeSum(x => x.PenaltySum).RegopRoundDecimal(2);
            var baseChangeSum = infos.SafeSum(x => x.BaseTariffChange).RegopRoundDecimal(2);
            var decChangeSum = infos.SafeSum(x => x.DecisionTariffChange).RegopRoundDecimal(2);
            var penaltyChangeSum = infos.SafeSum(x => x.PenaltyChange).RegopRoundDecimal(2);

            if (baseSum != 0)
            {
                changes.Add(this.historyCreator.CreateChange(account,
                    PersonalAccountChangeType.ChargeUndo,
                    "Отмена начислений по базовому тарифу за период(ы) {0} на сумму {1}".FormatUsing(periods, baseSum),
                    baseSum.ToString(),
                    baseSum.ToString(),
                    currentPeriod.StartDate,
                    changeInfo.Document,
                    changeInfo.Reason));
            }

            if (decSum != 0)
            {
                changes.Add(this.historyCreator.CreateChange(account,
                    PersonalAccountChangeType.ChargeUndo,
                    "Отмена начислений по тарифу решения за период(ы) {0} на сумму {1}".FormatUsing(periods, decSum),
                    decSum.ToString(),
                    decSum.ToString(),
                    currentPeriod.StartDate,
                    changeInfo.Document,
                    changeInfo.Reason));
            }

            if (penaltySum != 0)
            {
                changes.Add(this.historyCreator.CreateChange(account,
                    PersonalAccountChangeType.PenaltyUndo,
                    "Отмена начислений пени за период(ы) {0} на сумму {1}".FormatUsing(periods, penaltySum),
                    penaltySum.ToString(),
                    penaltySum.ToString(),
                    currentPeriod.StartDate,
                    changeInfo.Document,
                    changeInfo.Reason));
            }

            if (baseChangeSum != 0)
            {
                changes.Add(this.historyCreator.CreateChange(account,
                    PersonalAccountChangeType.ChargeUndo,
                    "Отмена ручных корректировок по базовому тарифу за период(ы) {0} на сумму {1}".FormatUsing(periods, baseChangeSum),
                    baseChangeSum.ToString(),
                    baseChangeSum.ToString(),
                    currentPeriod.StartDate,
                    changeInfo.Document,
                    changeInfo.Reason));
            }

            if (decChangeSum != 0)
            {
                changes.Add(this.historyCreator.CreateChange(account,
                    PersonalAccountChangeType.ChargeUndo,
                    "Отмена ручных корректировок по тарифу решения за период(ы) {0} на сумму {1}".FormatUsing(periods, decChangeSum),
                    decChangeSum.ToString(),
                    decChangeSum.ToString(),
                    currentPeriod.StartDate,
                    changeInfo.Document,
                    changeInfo.Reason));
            }

            if (penaltyChangeSum != 0)
            {
                changes.Add(this.historyCreator.CreateChange(account,
                    PersonalAccountChangeType.ChargeUndo,
                    "Отмена ручных корректировок пени за период(ы) {0} на сумму {1}".FormatUsing(periods, penaltyChangeSum),
                    penaltyChangeSum.ToString(),
                    penaltyChangeSum.ToString(),
                    currentPeriod.StartDate,
                    changeInfo.Document,
                    changeInfo.Reason));
            }

            return changes;
        }

        private class BalanceInfo
        {
            public decimal BaseTariff { get; set; }

            public decimal DecisionTariff { get; set; }

            public decimal Penalty { get; set; }
        }

        private class CancelParams
        {
            public bool CancelBaseTariffCharge { get; set; }
            public bool CancelBaseTariffChange { get; set; }
            public bool CancelDecisionTariffCharge { get; set; }
            public bool CancelDecisionTariffChange { get; set; }
            public bool CancelPenaltyCharge { get; set; }
            public bool CancelPenaltyChange { get; set; }

            public bool HasAnyFilter =>
                this.CancelBaseTariffCharge 
                || this.CancelBaseTariffChange 
                || this.CancelDecisionTariffCharge 
                || this.CancelDecisionTariffChange
                || this.CancelPenaltyCharge 
                || this.CancelPenaltyChange;

            public bool HasAnyChargeFilter => this.CancelBaseTariffCharge || this.CancelDecisionTariffCharge || this.CancelPenaltyCharge;

            public bool HasAnyChangeFilter => this.CancelBaseTariffChange || this.CancelDecisionTariffChange || this.CancelPenaltyChange;
        }
    }
}