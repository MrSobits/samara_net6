namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    using Utils = Bars.Gkh.RegOperator.Distribution.Utils;

    /// <summary>
    /// Абстрактное распределение по лицевым счетам
    /// </summary>
    public abstract class AbstractPersonalAccountDistribution : BaseDistribution
    {
        /// <summary>
        /// Домен-сервис "Импортируемая оплата"
        /// </summary>
        protected readonly IDomainService<ImportedPayment> importedPaymentDomain;

        /// <summary>
        /// Домен-сервис "Трансфер"
        /// </summary>
        protected readonly IDomainService<PersonalAccountPaymentTransfer> transferDomain;

        /// <summary>
        /// Домен-сервис <see cref="MoneyOperation" />
        /// </summary>
        protected readonly IDomainService<MoneyOperation> moneyOperationDomain;

        /// <summary>
        /// Домен-сервис "Лицевой счёт"
        /// </summary>
        protected readonly IDomainService<BasePersonalAccount> persaccDomain;

        /// <summary>
        /// Репозиторий для сущности "Период начислений"
        /// </summary>
        protected readonly IChargePeriodRepository chargePeriodRepo;

        /// <summary>
        /// Репозиторий для сущности "Трансфер"
        /// </summary>
        protected readonly ITransferRepository<PersonalAccountPaymentTransfer> transferRepo;

        /// <summary>
        /// Домен-сервис "Детализация распределения"
        /// </summary>
        protected readonly IDomainService<DistributionDetail> detailDomain;

        /// <summary>
        /// Домен-сервис "Счет оплат дома"
        /// </summary>
        protected readonly IDomainService<RealityObjectPaymentAccount> payAccDmn;

        /// <summary>
        /// Домен-сервис "Ситуация по ЛС на период"
        /// </summary>
        protected readonly IDomainService<PersonalAccountPeriodSummary> persAccSummaryDomain;

        /// <summary>
        /// Интерфейс для получения данных банка
        /// </summary>
        protected readonly IBankAccountDataProvider bankProvider;

        /// <summary>
        /// Домен-сервис <see cref="CalcAccountRealityObject" />
        /// </summary>
        protected readonly IDomainService<CalcAccountRealityObject> calcAccountRoDomain;

        /// <summary>
        /// Домен-сервис "Данные для документа на оплату по ЛС"
        /// </summary>
        protected readonly IDomainService<AccountPaymentInfoSnapshot> accountSnapshotDomain;
        
        public IDomainService<PaymentDocumentSnapshot> SnapshotDomain { get; set; }

        public IPaymentDocumentService PaymentDocumentService { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="transferDomain">Домен-сервис "Трансфер"</param>
        /// <param name="moneyOperationDomain">Домен-сервис <see cref="MoneyOperation" /></param>
        /// <param name="persaccDomain">Домен-сервис "Лицевой счёт"</param>
        /// <param name="chargePeriodRepo">Репозиторий для сущности "Период начислений"</param>
        /// <param name="transferRepo">Репозиторий для сущности "Трансфер"</param>
        /// <param name="importedPaymentDomain">Домен-сервис "Импортируемая оплата"</param>
        /// <param name="detailDomain">Домен-сервис "Детализация распределения"</param>
        /// <param name="payAccDmn">Домен-сервис "Счет оплат дома" </param>
        /// <param name="persAccSummaryDomain">Домен-сервис "Ситуация по ЛС на период"</param>
        /// <param name="accountSnapshotDomain">Домен-сервис "Данные для документа на оплату по ЛС"</param>
        /// <param name="bankProvider">Интерфейс для получения данных банка</param>
        /// <param name="calcAccountRoDomain">Домен-сервис "Жилой дом расчетного счета"</param>
        protected AbstractPersonalAccountDistribution(
            IDomainService<PersonalAccountPaymentTransfer> transferDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<BasePersonalAccount> persaccDomain,
            IChargePeriodRepository chargePeriodRepo,
            ITransferRepository<PersonalAccountPaymentTransfer> transferRepo,
            IDomainService<ImportedPayment> importedPaymentDomain,
            IDomainService<DistributionDetail> detailDomain,
            IDomainService<RealityObjectPaymentAccount> payAccDmn,
            IDomainService<PersonalAccountPeriodSummary> persAccSummaryDomain,
            IDomainService<AccountPaymentInfoSnapshot> accountSnapshotDomain,
            IBankAccountDataProvider bankProvider,
            IDomainService<CalcAccountRealityObject> calcAccountRoDomain)
        {
            this.transferDomain = transferDomain;
            this.moneyOperationDomain = moneyOperationDomain;
            this.persaccDomain = persaccDomain;
            this.chargePeriodRepo = chargePeriodRepo;
            this.transferRepo = transferRepo;
            this.importedPaymentDomain = importedPaymentDomain;
            this.detailDomain = detailDomain;
            this.payAccDmn = payAccDmn;
            this.persAccSummaryDomain = persAccSummaryDomain;
            this.bankProvider = bankProvider;
            this.accountSnapshotDomain = accountSnapshotDomain;
            this.calcAccountRoDomain = calcAccountRoDomain;
        }

        /// <summary>
        /// Роут клиентского контроллера
        /// </summary>
        public override string Route => "persaccdistribution";

        /// <summary>
        /// Проверка применимости распределения к счету НВС
        /// </summary>
        /// <param name="distributable">Распределяемый объект</param>
        /// <returns>Результат проверки</returns>
        public override bool CanApply(IDistributable distributable)
        {
            return distributable.MoneyDirection == MoneyDirection.Income;
        }

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="distributable">Распределяемый объект</param>
        /// <param name="operation">Отменяемая операция</param>
        public override void Undo(IDistributable distributable, MoneyOperation operation)
        {
            var cancelOperation = distributable.CancelOperation(operation, this.ChargePeriodRepository.GetCurrentPeriod());

            var command = this.GetCommand();

            var transferQuery = this.transferRepo.GetByMoneyOperation(operation);

            var accounts = this.GetPersonalAccounts(transferQuery, operation.Period).FetchCurrentOpenedPeriodSummary();

            var transfers = new List<Transfer>();

            var container = ApplicationContext.Current.Container;
            var walletDomain = container.ResolveDomain<Wallet>();

            foreach (var account in accounts)
            {
                var wallets = account.GetWallets();
                foreach (var wallet in wallets.Where(x => x.Id == 0))
                {
                    walletDomain.Save(wallet);
                }

                transfers.AddRange(account.UndoPayment(command, this.chargePeriodRepo.GetCurrentPeriod(), cancelOperation, distributable.DateReceipt));
            }

            this.moneyOperationDomain.Save(cancelOperation);
            this.moneyOperationDomain.Update(operation);
            transfers.ForEach(this.transferDomain.Save);
        }

        /// <summary>
        /// Возврат лицевых счетов по трансферам
        /// </summary>
        /// <param name="query">Запрос трансферов</param>
        /// <param name="period">Период</param>
        /// <returns>Перечисление аккаунтов</returns>
        public abstract IEnumerable<BasePersonalAccount> GetPersonalAccounts(IQueryable<Transfer> query, ChargePeriod period);

        /// <summary>
        /// Операция движения денег
        /// </summary>
        /// <returns>Исполняемая команда</returns>
        protected abstract IPersonalAccountPaymentCommand GetCommand();

        /// <summary>
        /// Извлечь интерфейс аргументов распределения из базовых параметров запроса
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns></returns>
        public override IDistributionArgs ExtractArgsFrom(BaseParams baseParams)
        {
            return DistributionByAccountsArgs.FromParams(baseParams);
        }

        /// <summary>
        /// Извлечь интерфейс аргументов распределения из базовых параметров запроса
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns></returns>
        public override IDistributionArgs ExtractArgsFromMany(BaseParams baseParams, int counter, decimal thisOneDistribSum)
        {
            return DistributionByAccountsArgs.FromManyParams(baseParams, counter, thisOneDistribSum);
        }

        /// <summary>
        /// Список объектов для распределения средств
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public override IDataResult ListDistributionObjs(BaseParams baseParams)
        {
            var distributionSum = baseParams.Params.GetAs<decimal?>("distribSum");

            var type = baseParams.Params.GetAs<SuspenseAccountDistributionParametersView>("distributionType", ignoreCase: true);

            var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);

            if (distributionSum == null)
            {
                distributionSum = distributable.RemainSum == 0 ? distributable.Sum : distributable.RemainSum;
            }

            var distribSum = distributionSum.ToDecimal();

            IQueryable<PersonalAccountDto> persAccQuery = this.ExtractAccountsQuery(baseParams);
            List<BasePersAccProxy> accounts = null;
            List<AccountDto> accountDtos = null;

            if (type == SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions)
            {
                accountDtos = this.ExtractAccountsQueryWithWallets(baseParams)
                    .Select(
                        x => new AccountDto
                        {
                            Id = x.Id,
                            PersonalAccountNum = x.PersonalAccountNum,
                            RoomAddress = x.RoomAddress,
                            AccountOwner = x.AccountOwner,
                            OwnerType = x.OwnerType,
                            State = x.State.Name,
                            BaseTariffWalletGuid = x.BaseTariffWalletGuid,
                            DecisionTariffWalletGuid = x.DecisionTariffWalletGuid,
                            PenaltyWalletGuid = x.PenaltyWalletGuid,
                            SocialSupportWalletGuid = x.SocialSupportWalletGuid,
                            AccumulatedFundWalletGuid = x.AccumulatedFundWalletGuid,
                            PreviosWorkPaymentWalletGuid = x.PreviosWorkPaymentWalletGuid,
                            RestructAmicableAgreementWalletGuid = x.RestructAmicableAgreementWalletGuid,
                            RoPaymentAccountNum = this.calcAccountRoDomain.GetAll()
                                .Where(y => y.Account.TypeAccount == TypeCalcAccount.Special || y.Account.TypeAccount == TypeCalcAccount.Regoperator)
                                .Where(y => y.RealityObject.Id == x.RoId)
                                .Where(y => y.Account.DateOpen <= DateTime.Today)
                                .Where(y => !y.Account.DateClose.HasValue || y.Account.DateClose.Value >= DateTime.Today)
                                .Where(
                                    y => (y.Account.TypeAccount == TypeCalcAccount.Special)
                                        || (y.Account.TypeAccount == TypeCalcAccount.Regoperator && y.DateStart <= DateTime.Today
                                            && (!y.DateEnd.HasValue || y.DateEnd >= DateTime.Today)))
                                .OrderByDescending(y => y.Account.DateOpen)
                                .Select(y => y.Account.AccountNumber ?? ((RegopCalcAccount)y.Account).ContragentCreditOrg.SettlementAccount)
                                .FirstOrDefault()
                        })
                    .ToList();
            }
            else
            {
                persAccQuery = this.ExtractAccountsQuery(baseParams);

                accounts = this.persaccDomain.GetAll()
                    .Fetch(x => x.AccountOwner)
                    .Fetch(x => x.Room)
                    .ThenFetch(x => x.RealityObject)
                    .Where(x => persAccQuery.Any(y => y.Id == x.Id))
                    .LeftJoin(
                        this.payAccDmn.GetAll(),
                        account => account.Room.RealityObject.Id,
                        account => account.RealityObject.Id,
                        (account, paymentAccount) => new BasePersAccProxy()
                        {
                            Account = account,
                            PaymentAccountNum = this.bankProvider.GetBankAccountNumber(paymentAccount.RealityObject)
                        })
                    .ToList();
            }

            var summaries = this.persAccSummaryDomain.GetAll()
                .Where(x => persAccQuery.Any(y => y.Id == x.PersonalAccount.Id))
                .ToList()
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, y => y.ToList());

            List<PersAccProxy> proxies;

            switch (type)
            {
                case SuspenseAccountDistributionParametersView.ProportionalArea:
                    var sumDistrib = accounts.SafeSum(x => x.Account.Room.Area * x.Account.AreaShare);

                    if (sumDistrib == 0m)
                    {
                        return new BaseDataResult(false,
                            "Невозможно выполнить распределение по площадям, сумма площадей равна нулю");
                    }

                    sumDistrib = distribSum / sumDistrib;

                    proxies = Utils.MoneyAndCentDistribution(
                        accounts,
                        x => x.Account.Room.Area * x.Account.AreaShare * sumDistrib,
                        distribSum,
                        (acc, sum) => new PersAccProxy(acc.Account, sum) { RoPayAccountNum = acc.PaymentAccountNum },
                        (proxy, coin) =>
                        {
                            if (proxy.Sum > 0)
                            {
                                proxy.Sum += coin;
                                return true;
                            }
                            else
                            {
                                proxy.Sum = 0;
                                return false;
                            }
                        });
                    break;

                case SuspenseAccountDistributionParametersView.Uniform:
                    proxies = Utils.MoneyAndCentDistribution(
                        accounts,
                        x => distribSum / accounts.Count,
                        distribSum,
                        (acc, sum) => new PersAccProxy(acc.Account, sum) { RoPayAccountNum = acc.PaymentAccountNum },
                        (proxy, coin) =>
                        {
                            if (proxy.Sum > 0)
                            {
                                proxy.Sum += coin;
                                return true;
                            }
                            else
                            {
                                proxy.Sum = 0;
                                return false;
                            }
                        });
                    break;

                case SuspenseAccountDistributionParametersView.ByDebt:
                    var debts =
                        accounts.Select(x => new
                            {
                                PersonalAccount = x.Account,
                                x.Account.GetOpenedPeriodSummary().SaldoOut,
                                x.PaymentAccountNum
                            })
                            .ToList();

                    if (debts.SafeSum(x => x.SaldoOut) >= distribSum)
                    {
                        proxies = Utils.MoneyAndCentDistribution(
                            debts,
                            x => x.SaldoOut <= 0
                                ? 0M
                                : distribSum * x.SaldoOut / debts.Where(y => y.SaldoOut > 0).Sum(y => y.SaldoOut),
                            distribSum,
                            (debt, sum) => new PersAccProxy(debt.PersonalAccount, sum) { RoPayAccountNum = debt.PaymentAccountNum },
                            (proxy, coin) =>
                            {
                                if (proxy.Sum > 0)
                                {
                                    proxy.Sum += coin;
                                    return true;
                                }
                                else
                                {
                                    proxy.Sum = 0;
                                    return false;
                                }
                            });
                    }
                    else
                    {
                        var extraMoney = distribSum - debts.SafeSum(x => x.SaldoOut);
                        var totalArea = accounts.SafeSum(x => x.Account.Room.Area);
                        var debtsByAccId = debts.GroupBy(x => x.PersonalAccount.Id)
                            .ToDictionary(x => x.Key, x => x.Single().SaldoOut);

                        proxies = Utils.MoneyAndCentDistribution(
                            accounts,
                            x => x.Account.Room.Area / totalArea * extraMoney + debtsByAccId[x.Account.Id],
                            distribSum,
                            (acc, sum) => new PersAccProxy(acc.Account, sum) { RoPayAccountNum = acc.PaymentAccountNum },
                            (proxy, coin) =>
                            {
                                if (proxy.Sum > 0)
                                {
                                    proxy.Sum += coin;
                                    return true;
                                }
                                else
                                {
                                    proxy.Sum = 0;
                                    return false;
                                }
                            });
                    }

                    break;

                case SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions:
                    var periodStartDate = baseParams.Params.GetAs<DateTime?>("periodStartDate");
                    var periodEndDate = baseParams.Params.GetAs<DateTime?>("periodEndDate");

                    if (periodStartDate == null || periodEndDate == null)
                    {
                        return new BaseDataResult(false, "Необходимо выбрать период для расчета накопленных взносов");
                    }

                    if (periodStartDate >= periodEndDate)
                    {
                        return new BaseDataResult(false,
                            "Дата начала периода не может быть больше даты окончания периода. Необходимо скорректировать даты");
                    }

                    if (accountDtos.Count == 0)
                    {
                        return new BaseDataResult(false, "Необходимо выбрать хотя бы 1 запись для распределения");
                    }

                    var inWalletGuids = accountDtos.SelectMany(
                            x => new[]
                            {
                                x.BaseTariffWalletGuid,
                                x.DecisionTariffWalletGuid,
                                x.PenaltyWalletGuid,
                                x.SocialSupportWalletGuid,
                                x.AccumulatedFundWalletGuid,
                                x.PreviosWorkPaymentWalletGuid,
                                x.RestructAmicableAgreementWalletGuid,
                            })
                        .ToArray();

                    var refundWalletGuids = accountDtos.SelectMany(
                            x => new[]
                            {
                                x.BaseTariffWalletGuid,
                                x.DecisionTariffWalletGuid,
                                x.PenaltyWalletGuid,
                                x.SocialSupportWalletGuid
                            })
                        .ToArray();

                    var inTransfers = this.transferDomain.GetAll()
                        .Fetch(x => x.Operation)
                        .Where(x => inWalletGuids.Contains(x.TargetGuid))
                        .Where(x => x.Operation.CanceledOperation == null)
                        .Where(x => x.PaymentDate.Date >= periodStartDate && x.PaymentDate.Date <= periodEndDate)
                        .Where(x => x.Amount != 0)
                        .GroupBy(x => x.TargetGuid)
                        .Select(x => new { x.Key, Amount = x.Sum(z => z.Amount) })
                        .Where(x => x.Amount != 0)
                        .ToDictionary(x => x.Key, x => x.Amount);

                    var refundTransfers = this.transferDomain.GetAll()
                        .Fetch(x => x.Operation)
                        .Where(x => refundWalletGuids.Contains(x.SourceGuid))
                        .Where(x => x.Operation.CanceledOperation == null)
                        .Where(x => x.PaymentDate.Date >= periodStartDate && x.PaymentDate.Date <= periodEndDate)
                        .Where(x => x.Reason.ToLower().Contains("возврат"))
                        .Where(x => x.Amount != 0)
                        .GroupBy(x => x.SourceGuid)
                        .Select(x => new { x.Key, Amount = x.Sum(z => z.Amount) })
                        .Where(x => x.Amount != 0)
                        .ToDictionary(x => x.Key, x => x.Amount);

                    var sumPeriodAccumulation = 0m;
                    foreach (var account in accountDtos)
                    {
                        var paymentsSum = this.GetTransfersSum(
                            inTransfers,
                            account.BaseTariffWalletGuid,
                            account.DecisionTariffWalletGuid,
                            account.PenaltyWalletGuid,
                            account.SocialSupportWalletGuid,
                            account.AccumulatedFundWalletGuid,
                            account.PreviosWorkPaymentWalletGuid,
                            account.RestructAmicableAgreementWalletGuid);

                        var refundsSum = this.GetTransfersSum(
                            refundTransfers,
                            account.BaseTariffWalletGuid,
                            account.DecisionTariffWalletGuid,
                            account.PenaltyWalletGuid,
                            account.SocialSupportWalletGuid);

                        // отрицательные накопления не учитываем
                        var periodAccumulation = paymentsSum - refundsSum;
                        if (periodAccumulation > 0)
                        {
                            account.PeriodAccumulation = periodAccumulation;
                            sumPeriodAccumulation += account.PeriodAccumulation;
                        }
                    }

                    proxies = Utils.MoneyAndCentDistribution(
                        accountDtos,
                        x => sumPeriodAccumulation > 0 ? (x.PeriodAccumulation * (distribSum / sumPeriodAccumulation)) : 0,
                        distribSum,
                        (accDto, sum) =>
                            new PersAccProxy(
                                accDto.Id,
                                accDto.PersonalAccountNum,
                                accDto.RoomAddress,
                                accDto.AccountOwner,
                                accDto.OwnerType,
                                accDto.State,
                                accDto.RoPaymentAccountNum,
                                sum,
                                0m,
                                accDto.PeriodAccumulation),
                        (proxy, coin) =>
                        {
                            if (proxy.Sum > 0)
                            {
                                proxy.Sum += coin;
                                return true;
                            }

                            return false;
                        },
                        (result, sumForEquals, roundedSum) =>
                        {
                            if (sumForEquals < roundedSum)
                            {
                                return result.OrderBy(x => x.PeriodAccumulation).ToList();
                            }

                            if (sumForEquals > roundedSum)
                            {
                                return result.OrderByDescending(x => x.PeriodAccumulation).ToList();
                            }

                            return result;
                        });

                    break;

                default:
                    proxies = Utils.MoneyAndCentDistribution(
                        accounts,
                        x => 0m,
                        0m,
                        (acc, sum) => new PersAccProxy(acc.Account, sum) { RoPayAccountNum = acc.PaymentAccountNum },
                        (proxy, coin) =>
                        {
                            if (proxy.Sum > 0)
                            {
                                proxy.Sum += coin;
                                return true;
                            }
                            else
                            {
                                proxy.Sum = 0;
                                return false;
                            }
                        });
                    break;
            }

            proxies.ForEach(x =>
            {
                var summary = summaries.Get(x.Id);
                x.Debt = summary.SafeSum(y => y.GetTotalCharge() - y.GetTotalPayment()).RegopRoundDecimal(2);
            });

            return new ListDataResult(proxies.OrderBy(x => x.Id), proxies.Count);
        }

        /// <summary>
        /// Вернуть список лицевых счетов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Перечисление ЛС</returns>
        public IQueryable<PersonalAccountDto> ExtractAccountsQuery(BaseParams baseParams)
        {
            if (baseParams.Params.GetAs<bool>("distributeUsingFilters"))
            {
                var container = ApplicationContext.Current.Container;
                var filterSvc = container.Resolve<IPersonalAccountFilterService>();
                var snapshotId = baseParams.Params.GetAsId("snapshotId");

                using (container.Using(filterSvc))
                {
                    var loadParam = baseParams.GetLoadParam();
                    var data = this.persaccDomain.GetAll()
                        .WhereIf(snapshotId > 0,
                            x => this.accountSnapshotDomain.GetAll()
                                .Where(z => z.Snapshot.Id == snapshotId)
                                .Any(z => z.AccountId == x.Id))
                        .ToDto()
                        .FilterByBaseParams(baseParams, filterSvc)
                        .Filter(loadParam, container);

                    return data;
                }
            }

            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

            return this.persaccDomain.GetAll()
                .Where(x => ids.Contains(x.Id))
                .ToDto();
        }

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="args">Интерфейс аргументов распределения</param>
        /// <returns></returns>
        protected IDataResult ApplyInternal(IDistributionArgs args)
        {
            var result = new BaseDataResult();
            var distrArgs = args as DistributionByAccountsArgs;
            if (distrArgs == null)
            {
                throw new ArgumentException("args should be convertible to DistributionByAccountsArgs");
            }

            var distributable = distrArgs.Distributable;
            var records = distrArgs.DistributionRecords;
            records.Select(x => x.PersonalAccount).FetchCurrentOpenedPeriodSummary();

            var roIds = records.Select(x => x.PersonalAccount.Room.RealityObject.Id).Distinct().ToArray();
            var roPaymentAccountNumDict = this.calcAccountRoDomain.GetAll()
                .Where(y => (y.Account.TypeAccount == TypeCalcAccount.Special && ((SpecialCalcAccount)y.Account).IsActive)
                    || y.Account.TypeAccount == TypeCalcAccount.Regoperator)
                .WhereContains(y => y.RealityObject.Id, roIds)
                .Where(y => y.Account.DateOpen <= DateTime.Today)
                .Where(y => !y.Account.DateClose.HasValue || y.Account.DateClose.Value >= DateTime.Today)
                .Where(
                    y => (y.Account.TypeAccount == TypeCalcAccount.Special)
                        || (y.Account.TypeAccount == TypeCalcAccount.Regoperator && y.DateStart <= DateTime.Today
                            && (!y.DateEnd.HasValue || y.DateEnd >= DateTime.Today)))
                .Select(y => new
                {
                    RoId = y.RealityObject.Id,
                    y.Account.DateOpen,
                    AccountNumber = y.Account.AccountNumber ?? ((RegopCalcAccount)y.Account).ContragentCreditOrg.SettlementAccount
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.DateOpen).First().AccountNumber);

            var operation = this.CreateMoneyOperation(distrArgs.Distributable);

            var period = this.chargePeriodRepo.GetCurrentPeriod();
            var paymentDate = distributable.DateReceipt;

            if (period == null)
            {
                throw new ArgumentException("Нет открытого периода");
            }

            var command = this.GetCommand();

            var amountDistrType = this.GetAmountDistributionType(distrArgs.Distributable);

            var transfers = new List<Transfer>();
            var details = new List<DistributionDetail>();
            foreach (var record in records)
            {
                if (record.Sum > 0 || record.SumPenalty > 0)
                {
                    var localTransfers =
                        record.PersonalAccount.ApplyPayment(
                            command,
                            new MoneyStream(
                                distrArgs.Distributable,
                                operation,
                                paymentDate,
                                distrArgs.SumDistribution != default ? distrArgs.SumDistribution / records.Count() : record.Sum + record.SumPenalty),
                            amountDistrType,
                            reserve: new AccountDistributionMoneyReserve { ByPenalty = record.SumPenalty });

                    localTransfers.Where(x => !x.IsInDirect).ForEach(x => details.Add(new DistributionDetail
                    {
                        Object = record.PersonalAccount.PersonalAccountNum,
                        Sum = x.Amount,
                        PaymentAccount = roPaymentAccountNumDict.Get(record.PersonalAccount.Room.RealityObject.Id)
                    }));

                    transfers.AddRange(localTransfers);
                }
            }

            this.moneyOperationDomain.Save(operation);

            transfers.ForEach(this.transferDomain.Save);

            details.GroupBy(x => new { x.Object, x.PaymentAccount })
                .Select(x => new DistributionDetail(distributable.Id, x.Key.Object, x.Sum(z => z.Sum))
                {
                    Source = distributable.Source,
                    PaymentAccount = x.Key.PaymentAccount,
                    Destination = "Распределение оплаты"
                })
                .ForEach(this.detailDomain.Save);

            if (distrArgs.TypeDistribution == SuspenseAccountDistributionParametersView.ByPaymentDocument)
            {
                this.SetStatusSnapshot(distrArgs);
            }

            return result;
        }

        private void SetStatusSnapshot(DistributionByAccountsArgs distrArgs)
        {
            var snapshotId = distrArgs.Params.Get("snapshotId").ToLong();

            var distribSum = distrArgs.DistributionRecords.SafeSum(x => x.Sum + x.SumPenalty);

            var snapshot = this.SnapshotDomain.Get(snapshotId);

            var snapshotSum = this.PaymentDocumentService.GetPaymentInfoSnapshots(snapshotId)
                .SafeSum(x => x.ChargeSum + x.PenaltySum);

            snapshot.PaymentState = distribSum >= snapshotSum ? PaymentDocumentPaymentState.Paid : PaymentDocumentPaymentState.PartiallyPaid;

            this.SnapshotDomain.Save(snapshot);
        }

        /// <summary>
        /// Вернуть тип распределения суммы оплаты из распределяемого объекта
        /// </summary>
        /// <param name="distributable">Распределяемый объект</param>
        /// <returns>Тип распределения</returns>
        protected virtual AmountDistributionType GetAmountDistributionType(IDistributable distributable)
        {
            var payment = this.importedPaymentDomain.GetAll()
                .Where(x => x.PaymentId == distributable.Id)
                .Where(x => x.PaymentState == ImportedPaymentState.Rns)
                .Select(x => (YesNo?)x.BankDocumentImport.DistributePenalty)
                .FirstOrDefault();

            return payment.HasValue && payment.Value == YesNo.Yes
                ? AmountDistributionType.TariffAndPenalty
                : AmountDistributionType.Tariff;
        }

        /// <summary>
        /// Вернуть список лицевых счетов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Перечисление ЛС</returns>
        protected IQueryable<PersonalAccountWalletDto> ExtractAccountsQueryWithWallets(BaseParams baseParams)
        {
            if (baseParams.Params.GetAs<bool>("distributeUsingFilters"))
            {
                var container = ApplicationContext.Current.Container;
                var filterSvc = container.Resolve<IPersonalAccountFilterService>();
                var snapshotId = baseParams.Params.GetAsId("snapshotId");

                using (container.Using(filterSvc))
                {
                    var loadParam = baseParams.GetLoadParam();
                    var data = this.persaccDomain.GetAll()
                        .WhereIf(snapshotId > 0,
                            x => this.accountSnapshotDomain.GetAll()
                                .Where(z => z.Snapshot.Id == snapshotId)
                                .Any(z => z.AccountId == x.Id))
                        .ToDtoWithWallets()
                        .FilterByBaseParams(baseParams, filterSvc)
                        .Filter(loadParam, container);

                    return data;
                }
            }

            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

            return this.persaccDomain.GetAll()
                .Where(x => ids.Contains(x.Id))
                .ToDtoWithWallets();
        }

        private decimal GetTransfersSum(Dictionary<string, decimal> transfers, params string[] walletIds)
        {
            return walletIds.Sum(x => transfers.Get(x));
        }

        /// <summary>
        /// Прокси лицевого счёта
        /// </summary>
        protected class PersAccProxy
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            public PersAccProxy()
            {
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="persAccount">Лицевой счёт</param>
            /// <param name="sum">Сумма</param>
            /// <param name="sumPenalty">Сумма пени</param>
            public PersAccProxy(BasePersonalAccount persAccount, decimal sum = 0M, decimal sumPenalty = 0M, decimal periodAccumulation = 0M)
            {
                this.Id = persAccount.Id;
                this.PersonalAccountNum = persAccount.PersonalAccountNum;
                this.RoomAddress = $"{persAccount.Room.RealityObject.Address}, кв. {persAccount.Room.RoomNum}"
                    + $"{(persAccount.Room.ChamberNum.IsNotEmpty() ? ", ком. " + persAccount.Room.ChamberNum : string.Empty)}";

                this.AccountOwner = persAccount.AccountOwner.Name;
                this.OwnerType = persAccount.AccountOwner.OwnerType;
                this.Sum = sum;
                this.SumPenalty = sumPenalty;
                this.State = persAccount.State.Return(x => x.Name);
                this.PeriodAccumulation = periodAccumulation;
            }

            public PersAccProxy(
                long accountId,
                string accountNum,
                string roomAddress,
                string accountOwnerName,
                PersonalAccountOwnerType accountOwnerType,
                string stateName,
                string roPayAccountNum,
                decimal sum = 0M,
                decimal sumPenalty = 0M,
                decimal periodAccumulation = 0M)
            {
                this.Id = accountId;
                this.PersonalAccountNum = accountNum;
                this.RoomAddress = roomAddress;
                this.AccountOwner = accountOwnerName;
                this.OwnerType = accountOwnerType;
                this.Sum = sum;
                this.SumPenalty = sumPenalty;
                this.State = stateName;
                this.PeriodAccumulation = periodAccumulation;
                this.RoPayAccountNum = roPayAccountNum;
            }

            /// <summary>
            /// Идентификатор
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Состояние
            /// </summary>
            public string State { get; set; }

            /// <summary>
            /// Номер лицевого счёта
            /// </summary>
            public string PersonalAccountNum { get; set; }

            /// <summary>
            /// Адрес квартиры
            /// </summary>
            public string RoomAddress { get; set; }

            /// <summary>
            /// Владелец лицевого счёта
            /// </summary>
            public string AccountOwner { get; set; }

            /// <summary>
            /// Тип абонента
            /// </summary>
            public PersonalAccountOwnerType OwnerType { get; set; }

            /// <summary>
            /// Задолженность
            /// </summary>
            public decimal Debt { get; set; }

            /// <summary>
            /// Сумма
            /// </summary>
            public decimal Sum { get; set; }

            /// <summary>
            /// Сумма пени
            /// </summary>
            public decimal SumPenalty { get; set; }

            /// <summary>
            /// Накопления за период
            /// </summary>
            public decimal PeriodAccumulation { get; set; }

            /// <summary>
            /// Номер расчётного счета банка, к которому привязан ЛС
            /// </summary>
            public string RoPayAccountNum { get; set; }
        }

        /// <summary>
        /// Класс-прокси для персонального аккаунта ЛС
        /// </summary>
        protected class BasePersAccProxy
        {
            /// <summary>
            /// Лицевой счёт абонента
            /// </summary>
            public BasePersonalAccount Account { get; set; }

            /// <summary>
            /// Номер расчётного счета банка, к которому привязан ЛС
            /// </summary>
            public string PaymentAccountNum { get; set; }
        }

        protected class AccountDto
        {
            public long Id { get; set; }

            public string State { get; set; }

            public string PersonalAccountNum { get; set; }

            public string RoomAddress { get; set; }

            public string AccountOwner { get; set; }

            public PersonalAccountOwnerType OwnerType { get; set; }

            public string RoPaymentAccountNum { get; set; }

            public decimal PeriodAccumulation { get; set; }

            public string BaseTariffWalletGuid { get; set; }

            public string DecisionTariffWalletGuid { get; set; }

            public string PenaltyWalletGuid { get; set; }

            public string SocialSupportWalletGuid { get; set; }

            public string AccumulatedFundWalletGuid { get; set; }

            public string PreviosWorkPaymentWalletGuid { get; set; }

            public string RestructAmicableAgreementWalletGuid { get; set; }
        }
    }
}