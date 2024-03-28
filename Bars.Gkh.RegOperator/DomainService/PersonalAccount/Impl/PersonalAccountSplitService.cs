namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.Interfaces;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.PerformanceLogging;

    using Castle.MicroKernel;
    using Castle.Windsor;

    using FastMember;

    using Newtonsoft.Json;

    using NHibernate.Linq;

    using Utils = Bars.Gkh.RegOperator.Distribution.Utils;

    /// <summary>
    /// Сервис разедения ЛС
    /// </summary>
    public class PersonalAccountSplitService : IPersonalAccountSplitService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        public IDomainService<PersonalAccountOwner> PersonalAccountOwnerDomain { get;set; }

        public IDomainService<IndividualAccountOwner> IndividualAccountOwnerDomain { get;set; }

        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }

        public ITransferRepository TransferRepository { get; set; }

        public IPersonalAccountFactory PersonalAccountFactory { get;set; }

        public IPersonalAccountChangeService PersonalAccountChangeService { get;set; }

        public IMassPersonalAccountDtoService MassPersonalAccountDtoService { get; set; }

        public IFileManager FileManager { get; set; }

        /// <inheritdoc />
        public IDataResult GetDistributableDebts(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var splitDate = baseParams.Params.GetAs<DateTime>("splitDate");
            var savePayments = baseParams.Params.GetAs<bool>("savePayments");

            var account = this.BasePersonalAccountDomain.GetAll()
                .Fetch(x => x.BaseTariffWallet)
                .Fetch(x => x.DecisionTariffWallet)
                .Fetch(x => x.PenaltyWallet)
                .Fetch(x => x.Room)
                .ThenFetch(x => x.RealityObject)
                .FetchMany(x => x.Summaries)
                .Where(x => x.Id == id)
                .ToList()
                .FirstOrDefault();

            if (account.IsNull())
            {
                return BaseDataResult.Error("Ошибка при получении лицевого счета");
            }

            if (account.State.Code != "1")
            {
                return BaseDataResult.Error("Необходимо выбрать открытый лицевой счет!");
            }

            if (!splitDate.IsValid() || splitDate < account.OpenDate)
            {
                return BaseDataResult.Error("Дата разделения не должна быть ранее даты открытия существующего ЛС");
            }

            var performanceLoggerFactory = this.Container.Resolve<IPerformanceLoggerFactory>();
            var performanceLogger = performanceLoggerFactory.GetLogger();
            var collector = performanceLoggerFactory.GetCollector();

            var parameterTracker = this.Container.Resolve<IParameterTracker>("OnDateParameterTracker");
            var globalCache = this.Container.Resolve<ICalculationGlobalCache>(new Arguments
            {
                {"performanceLogger", performanceLogger}
            });
            var periodRepository = this.Container.Resolve<IChargePeriodRepository>();
            var chargeDomain = this.Container.ResolveDomain<PersonalAccountCharge>();

            var argument = new Arguments { { "paramTracker", parameterTracker } };
            var factory = this.Container.Resolve<IChargeCalculationImplFactory>(argument);

            var period = periodRepository.GetCurrentPeriod();

            UnacceptedCharge charge;
            var summary = account.GetOpenedPeriodSummary();

            // исходящие сальдо без учета начислений, мы всегда успеем их прибавить
            // зато потом их трудно будет учесть после расчета
            var baseTariffEndDebt = summary.BaseTariffDebt + summary.GetBaseTariffDebt();
            var decisionTariffEndDebt = summary.DecisionTariffDebt + summary.GetDecisionTariffDebt();
            var penaltyEndDebt = summary.PenaltyDebt + summary.GetPenaltyDebt();

            using (this.Container.Using(
                globalCache, 
                factory, 
                parameterTracker, 
                chargeDomain,
                performanceLoggerFactory,
                performanceLogger,
                collector))
            {
                globalCache.SetManualRecalcDate(account, splitDate);
                globalCache.Initialize(period, new[] { account }.AsQueryable());
                charge = account.CreateCharge(period, new UnacceptedChargePacket(), factory, splitDate);
                globalCache.Clear();

                var activeChargeInPeriod = chargeDomain.GetAll()
                    .FirstOrDefault(x => x.ChargePeriod == period && x.IsActive && x.BasePersonalAccount == account);

                // вычитаем начисления за текущий период, т.к. потом их заново прибавим
                baseTariffEndDebt -= (activeChargeInPeriod?.ChargeTariff - activeChargeInPeriod?.OverPlus + activeChargeInPeriod?.RecalcByBaseTariff) ?? 0;
                decisionTariffEndDebt -= (activeChargeInPeriod?.OverPlus + activeChargeInPeriod?.RecalcByDecisionTariff) ?? 0;
                penaltyEndDebt -= (activeChargeInPeriod?.Penalty - activeChargeInPeriod?.RecalcPenalty) ?? 0;
            }

            var baseTariffCharges = charge.ChargeTariff - charge.TariffOverplus + charge.RecalcByBaseTariff;
            var decisionTariffCharges = charge.TariffOverplus + charge.RecalcByDecision;
            var penaltyCharges = charge.Penalty + charge.RecalcPenalty;

            // прибавляем начисления + перерасчеты после даты закрытия
            var resultBaseTariffDebt = baseTariffEndDebt + baseTariffCharges;
            var resultDecisionTariffDebt = decisionTariffEndDebt + decisionTariffCharges;
            var resultPenaltyDebt = penaltyEndDebt + penaltyCharges;

         if (savePayments)
           {
                //var walletDict = account.GetMainWalletsDict();
                //var guids = walletDict.Select(x => x.Value.WalletGuid).ToArray();


                //var transfers = this.TransferRepository.QueryOver<PersonalAccountPaymentTransfer>()
                //    .Where(x => x.Owner == account
                //        && !x.Operation.IsCancelled
                //        && x.Operation.CanceledOperation == null
                //        && x.IsAffect)
                //    .WhereContains(x => x.TargetGuid, guids)
                //    .Where(x => x.PaymentDate >= splitDate)
                //    .Select(x => new
                //    {
                //        x.TargetGuid,
                //        x.Amount
                //    })
                //    .AsEnumerable()
                //    .GroupBy(x => x.TargetGuid, x => x.Amount)
                //    .ToDictionary(x => this.ExtractWalletType(walletDict, x.Key), x => x.Sum());

                //resultBaseTariffDebt += transfers.Get(WalletType.BaseTariffWallet);
                //resultDecisionTariffDebt += transfers.Get(WalletType.DecisionTariffWallet);
                //resultPenaltyDebt += transfers.Get(WalletType.PenaltyWallet);

                //вернуть обратно если не прокатит
                //resultBaseTariffDebt = baseTariffEndDebt;
                //resultDecisionTariffDebt = decisionTariffEndDebt;
                //resultPenaltyDebt = penaltyEndDebt;

                resultBaseTariffDebt = 0;
                resultDecisionTariffDebt = 0;
                resultPenaltyDebt = 0;

            }

            return new BaseDataResult(new DebtInfo
            {
                BaseTariffDebt = resultBaseTariffDebt,
                DecisionTariffDebt = resultDecisionTariffDebt,
                PenaltyDebt = resultPenaltyDebt
            });
        }

        /// <inheritdoc />
        public IDataResult DistributeDebts(BaseParams baseParams)
        {
            var accounts = baseParams.Params["records"]
                .As<List<object>>()
                .Select(x => x.As<DynamicDictionary>().ReadClass<Record>())
                .ToArray();

            var debts = baseParams.Params["debts"]
                .As<DynamicDictionary>()
                .ReadClass<DebtInfo>();

            var distributionType = baseParams.Params.GetAs<SplitAccountDistributionType>("distributionType");

            // сначала распределяем задолженности по базовому тарифу
            var proxies = DistributeBaseTariffDebt(debts, distributionType, accounts, baseParams);
            proxies = PersonalAccountSplitService.DistributeDecisionTariffDebt(debts, distributionType, proxies);
            proxies = PersonalAccountSplitService.DistributePenaltyDebt(debts, distributionType, proxies);

            return new ListDataResult(proxies, proxies.Count);
        }

        /// <inheritdoc />
        public IDataResult ApplyDistribution(BaseParams baseParams)
        {
            var accounts = baseParams.Params["records"]
                .As<List<object>>()
                .Select(x => x.As<DynamicDictionary>().ReadClass<Record>())
                .ToArray();

            var splitDate = baseParams.Params.GetAs<DateTime>("SplitDate");
            var sourceAccount = accounts.FirstOrDefault(x => x.Id > 0);

            var account = this.BasePersonalAccountDomain.Get(sourceAccount?.Id ?? 0);

            if (account.IsNull())
            {
                return BaseDataResult.Error("Ошибка при получении лицевого счета");
            }

            if (account.State.Code != "1")
            {
                return BaseDataResult.Error("Необходимо выбрать открытый лицевой счет!");
            }

            if (!splitDate.IsValid() || splitDate < account.OpenDate)
            {
                return BaseDataResult.Error("Дата разделения не должна быть ранее даты открытия существующего ЛС");
            }

            return this.DistributeInternal(baseParams, account, accounts, splitDate);
        }

        private IDataResult DistributeInternal(BaseParams baseParams, BasePersonalAccount account, Record[] accounts, DateTime splitDate)
        {
            var chargePeriodRepository = this.Container.Resolve<IChargePeriodRepository>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var stateRepo = this.Container.Resolve<IStateRepository>();
            var moneyOperationDomain = this.Container.ResolveDomain<MoneyOperation>();
            var walletDomain = this.Container.ResolveDomain<Wallet>();
            var periodSummaryDomain = this.Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var splitAccountDomain = this.Container.ResolveDomain<SplitAccountSource>();
            var entityLogLightDomain = this.Container.ResolveDomain<EntityLogLight>();
            var chnageDomain = this.Container.ResolveDomain<PersonalAccountChange>();
            var savePayments = baseParams.Params.GetAs<bool>("savePayments");
            var reason = baseParams.Params.GetAs<string>("Reason", ignoreCase: true);
            var closeCurrent = baseParams.Params.GetAs<bool>("closeCurrentAccount");

            using (this.Container.Using(
                chargePeriodRepository, 
                walletDomain, 
                moneyOperationDomain,
                splitAccountDomain,
                periodSummaryDomain,
                userManager,
                entityLogLightDomain,
                chnageDomain,
                stateRepo))
            {
                IDataResult result = null;
                this.Container.InTransaction(() =>
                {
                    FileInfo fileInfo = null;
                    if (baseParams.Files.Any())
                    {
                        fileInfo = this.FileManager.SaveFile(baseParams.Files.First().Value);
                    }

                    // создаем источник операции
                    var period = chargePeriodRepository.GetCurrentPeriod();
                    var splitSource = new SplitAccountSource(account, period, splitDate)
                    {
                        Document = fileInfo,
                        User = userManager.GetActiveUser()?.Login ?? "anonymous",
                        Reason = reason
                    };

                    // подготавливаем данные дле переноса долга
                    var recipientItems = accounts.Where(x => x.Id != account.Id).ToArray();
                    var operation = splitSource.CreateOperation(period);

                    var operationResult = new PersonalAccountOperationResult(operation);
                    var accountsToSave = new List<BasePersonalAccount>();

                    // изменяем долю собственности ЛС
                    // сначала изменяем долю собственности, перед созданием новых ЛС, иначе потом не пройдет проверка
                    account.AreaShare = accounts.First(x => x.Id == account.Id).NewAreaShare;
                    if (closeCurrent)
                    {
                        if (account.AreaShare != 0)
                        {
                            result = BaseDataResult.Error("Доля собственности закрываемого лицевого счета должна быть равна нулю!");
                        }

                        var hasDebt = account.GetOpenedPeriodSummary().SaldoOut > 0;

                        account.SetCloseDate(splitDate, false);
                        account.State = stateRepo.GetAllStates<BasePersonalAccount>()
                            .WhereIf(hasDebt, x => x.Code == "3")
                            .WhereIf(!hasDebt, x => x.Code == "2")
                            .First();
                    }

                    this.BasePersonalAccountDomain.Update(account);

                    foreach (var recipientItem in recipientItems)
                    {
                        var accountCreateInfo = this.GetCreateInfo(recipientItem);

                        entityLogLightDomain.Save(new EntityLogLight
                        {
                            ClassName = "BasePersonalAccount",
                            EntityId = account.Id,
                            PropertyDescription = "Изменение доли собственности в связи с разделением лицевого счета",
                            PropertyName = "AreaShare",
                            PropertyValue = account.AreaShare.ToStr(),
                            DateActualChange = splitSource.OperationDate,
                            DateApplied = DateTime.UtcNow,
                            Document = fileInfo,
                            Reason = reason,
                            ParameterName = "area_share",
                            User = splitSource.User
                        });

                        var targetAccount = this.PersonalAccountFactory.CreateNewAccount(
                            accountCreateInfo.AccountOwner,
                            account.Room,
                            accountCreateInfo.OpenDate,
                            accountCreateInfo.AreaShare);

                        this.MassPersonalAccountDtoService.AddPersonalAccount(targetAccount);
                        // todo: вернуться сюда после того как разберемся с задолженностью в расщеплении
                        targetAccount.GetWallets().ForEach(x => walletDomain.Save(x));
                        this.BasePersonalAccountDomain.Save(targetAccount);
                        
                        splitSource
                            .SplitTo(
                            targetAccount, 
                                recipientItem.NewBaseTariffDebt,
                                recipientItem.NewDecisionTariffDebt,
                                recipientItem.NewPenaltyDebt)
                            .ForEach(x => operationResult.AddTransfer(x));

                        if (savePayments)
                        {
                            chnageDomain.Save(new PersonalAccountChange(
                                targetAccount,
                                $"Распределение задолженности с лицевого счета {account.PersonalAccountNum} с даты {splitDate.ToShortDateString()}",
                                PersonalAccountChangeType.SplitAccount,
                                DateTime.Now,
                                splitDate,
                                splitSource.User,
                                "0",
                                null,
                                period)
                            {
                                Document = fileInfo
                            });
                        }
                        else
                        {
                            chnageDomain.Save(new PersonalAccountChange(
                                targetAccount,
                                $"Распределение задолженности с лицевого счета {account.PersonalAccountNum} с даты {splitDate.ToShortDateString()}",
                                PersonalAccountChangeType.SplitAccount,
                                DateTime.Now,
                                splitDate,
                                splitSource.User,
                                recipientItem.NewDebtSum.RegopRoundDecimal(2).ToStr(),
                                null,
                                period)
                            {
                                Document = fileInfo
                            });
                        }

                        accountsToSave.Add(targetAccount);
                    }

                    chnageDomain.Save(new PersonalAccountChange(
                        account,
                        $"Распределение задолженности с лицевого счета {account.PersonalAccountNum} с даты {splitDate.ToShortDateString()}"
                        + $" на лицевые счета: {accountsToSave.AggregateWithSeparator(x => x.PersonalAccountNum, ", ")}",
                        PersonalAccountChangeType.SplitAccount,
                        DateTime.Now,
                        splitDate,
                        splitSource.User,
                        recipientItems.Sum(x => x.NewDebtSum).RegopRoundDecimal(2).ToStr(),
                        null,
                        period)
                    {
                        Document = fileInfo
                    });

                    splitAccountDomain.Save(splitSource);
                    moneyOperationDomain.Save(operationResult.Operation);
                    operationResult.Transfers.ForEach(x => this.TransferRepository.SaveOrUpdate(x));
                    periodSummaryDomain.Update(account.GetOpenedPeriodSummary());
                    accountsToSave.ForEach(x =>
                    {
                        this.BasePersonalAccountDomain.Update(x);
                        periodSummaryDomain.Update(x.GetOpenedPeriodSummary());
                    });
                });

                if (result.IsNotNull() && !result.Success)
                {
                    this.MassPersonalAccountDtoService.Clear();
                    return result;
                }

                this.MassPersonalAccountDtoService.ApplyChanges();
                return new BaseDataResult();
            }
        }

        private IList<Record> DistributeBaseTariffDebt(DebtInfo debts, SplitAccountDistributionType distributionType, IList<Record> accounts, BaseParams baseParams)
        {            
            var proxies = new List<Record>();
            var distributionSum = Math.Abs(debts.BaseTariffDebt);
            var sign = Math.Sign(debts.BaseTariffDebt);
            long id = 0;
            decimal totalCharge = 0;
            decimal totalPayments = 0;
            decimal totalPaymentsWithLastOP = 0;
            var savePayments = baseParams.Params.GetAs<bool>("savePayments");

            if (sign == 0)
            {
                return accounts;
            }

            foreach (Record record in accounts)
            {
                if (record.Id > 0)
                {
                    var basePersonalAccount = BasePersonalAccountDomain.Get(record.Id);
                    var summary = basePersonalAccount.GetOpenedPeriodSummary();
               
                    var baseTariffEndDebt = summary.BaseTariffDebt;
                    var decisionTariffEndDebt = summary.DecisionTariffDebt;
                    var penaltyEndDebt = summary.PenaltyDebt;

                    var walletDict = basePersonalAccount.GetMainWalletsDict();
                    var guids = walletDict.Select(x => x.Value.WalletGuid).ToArray();

                    totalCharge = baseTariffEndDebt;
                    
                    var transfers = this.TransferRepository.QueryOver<PersonalAccountPaymentTransfer>()
                        .Where(x => x.Owner == basePersonalAccount
                            && !x.Operation.IsCancelled
                            && x.Operation.CanceledOperation == null
                            && x.IsAffect)
                        .WhereContains(x => x.TargetGuid, guids)
                        .Select(x => new
                        {
                            x.TargetGuid,
                            x.Amount
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.TargetGuid, x => x.Amount)
                        .ToDictionary(x => this.ExtractWalletType(walletDict, x.Key), x => x.Sum());

                    var resultBaseTariffPayments = transfers.Get(WalletType.BaseTariffWallet);
                    var resultDecisionTariffPayments = transfers.Get(WalletType.DecisionTariffWallet);
                    var resultPenaltyPayments = transfers.Get(WalletType.PenaltyWallet);

                    totalPayments = resultBaseTariffPayments - summary.TariffPayment;
                    totalPaymentsWithLastOP = resultBaseTariffPayments;

                }
            }

            switch (distributionType)
            {
                case SplitAccountDistributionType.Uniform:

                    proxies = Utils.MoneyAndCentDistribution(
                        accounts,
                        x => distributionSum / accounts.Count,
                        distributionSum,
                        (acc, sum) =>
                        {
                            acc.NewBaseTariffDebt = sum;
                            return acc;
                        },
                        (proxy, coin) =>
                        {
                            proxy.NewBaseTariffDebt += coin;
                            return true;
                        });

                    break;

                case SplitAccountDistributionType.ProportionalAreaShare:

                    //if (savePayments)
                    //{
                    //    foreach (Record record in accounts)
                    //    {
                    //        if (record.Id > 0)
                    //        {
                    //            record.NewBaseTariffDebt  = (totalCharge + totalPayments) * record.NewAreaShare - totalPaymentsWithLastOP;
                    //        }
                    //        else
                    //        {
                    //            record.NewBaseTariffDebt  = (totalCharge + totalPayments) * record.NewAreaShare;
                    //        }
                    //        proxies.Add(record);
                    //    }
                    //    break;
                    //}
                    if (savePayments)
                    {
                        foreach (Record record in accounts)
                        {
                            if (record.Id > 0)
                            {
                                record.NewBaseTariffDebt = 0;
                            }
                            else
                            {
                                record.NewBaseTariffDebt = 0;
                            }
                            proxies.Add(record);
                        }
                        break;
                    }
                    else
                    {

                        var sumDistrib = accounts.SafeSum(x => x.NewAreaShare);

                        sumDistrib = distributionSum / sumDistrib;

                        proxies = Utils.MoneyAndCentDistribution(
                            accounts,
                            x => x.NewAreaShare * sumDistrib,
                            distributionSum,
                            (acc, sum) =>
                            {
                                acc.NewBaseTariffDebt = sum;
                                return acc;
                            },
                            (proxy, coin) =>
                            {
                                proxy.NewBaseTariffDebt += coin;
                                return true;
                            });
                        break;
                    }
            }

          //  proxies.ForEach(x => x.NewBaseTariffDebt *= sign);

            return proxies;
        }
        private static IList<Record> DistributeDecisionTariffDebt(DebtInfo debts, SplitAccountDistributionType distributionType, IList<Record> accounts)
        {
            var proxies = new List<Record>();
            var distributionSum = Math.Abs(debts.DecisionTariffDebt);
            var sign = Math.Sign(debts.DecisionTariffDebt);

            if (sign == 0)
            {
                return accounts;
            }

            switch (distributionType)
            {
                case SplitAccountDistributionType.Uniform:

                    proxies = Distribution.Utils.MoneyAndCentDistribution(
                        accounts,
                        x => distributionSum / accounts.Count,
                        distributionSum,
                        (acc, sum) =>
                        {
                            acc.NewDecisionTariffDebt = sum;
                            return acc;
                        },
                        (proxy, coin) =>
                        {
                            proxy.NewDecisionTariffDebt += coin;
                            return true;
                        });

                    break;

                case SplitAccountDistributionType.ProportionalAreaShare:
                    var sumDistrib = accounts.SafeSum(x => x.NewAreaShare);

                    sumDistrib = distributionSum / sumDistrib;

                    proxies = Utils.MoneyAndCentDistribution(
                        accounts,
                        x => x.NewAreaShare * sumDistrib,
                        distributionSum,
                        (acc, sum) =>
                        {
                            acc.NewDecisionTariffDebt = sum;
                            return acc;
                        },
                        (proxy, coin) =>
                        {
                            proxy.NewDecisionTariffDebt += coin;
                            return true;
                        });
                    break;
            }

            proxies.ForEach(x => x.NewDecisionTariffDebt *= sign);
            return proxies;
        }
        private static IList<Record> DistributePenaltyDebt(DebtInfo debts, SplitAccountDistributionType distributionType, IList<Record> accounts)
        {
            var proxies = new List<Record>();
            var distributionSum = Math.Abs(debts.PenaltyDebt);
            var sign = Math.Sign(debts.PenaltyDebt);

            if (sign == 0)
            {
                return accounts;
            }

            switch (distributionType)
            {
                case SplitAccountDistributionType.Uniform:

                    proxies = Utils.MoneyAndCentDistribution(
                        accounts,
                        x => distributionSum / accounts.Count,
                        distributionSum,
                        (acc, sum) =>
                        {
                            acc.NewPenaltyDebt = sum;
                            return acc;
                        },
                        (proxy, coin) =>
                        {
                            proxy.NewPenaltyDebt += coin;
                            return true;
                        });

                    break;

                case SplitAccountDistributionType.ProportionalAreaShare:
                    var sumDistrib = accounts.SafeSum(x => x.NewAreaShare);

                    sumDistrib = distributionSum / sumDistrib;

                    proxies = Utils.MoneyAndCentDistribution(
                        accounts,
                        x => x.NewAreaShare * sumDistrib,
                        distributionSum,
                        (acc, sum) =>
                        {
                            acc.NewPenaltyDebt = sum;
                            return acc;
                        },
                        (proxy, coin) =>
                        {
                            proxy.NewPenaltyDebt += coin;
                            return true;
                        });
                    break;
            }

            proxies.ForEach(x => x.NewPenaltyDebt *= sign);

            return proxies;
        }

        private WalletType ExtractWalletType(IDictionary<WalletType, Wallet> walletDict, string walletGuid)
        {
            return walletDict.First(x => x.Value.WalletGuid == walletGuid).Key;
        }

        private PersonalAccountCreateInfo GetCreateInfo(Record record)
        {
            PersonalAccountOwner owner;

            if (record.OwnerType == PersonalAccountOwnerType.Individual)
            {
                if (record.AccountOwner.HasValue)
                {
                    owner = this.IndividualAccountOwnerDomain.Get(record.AccountOwner.Value);
                }
                else
                {
                    owner = new IndividualAccountOwner
                    {
                        FirstName = record.FirstName,
                        SecondName = record.SecondName,
                        Surname = record.Surname
                    };

                    this.IndividualAccountOwnerDomain.Save(owner);
                }
            }
            else
            {
                owner = this.LegalAccountOwnerDomain.GetAll().FirstOrDefault(x => x.Contragent.Id == record.Contragent);
                if (owner.IsNull() && record.Contragent.HasValue)
                {
                    owner = new LegalAccountOwner
                    {
                        Contragent = new Contragent { Id = record.Contragent.Value }
                    };

                    this.LegalAccountOwnerDomain.Save(owner);
                }
            }

            return new PersonalAccountCreateInfo
            {
                OpenDate = record.OpenDate,
                AreaShare = record.NewAreaShare,
                AccountOwner = owner,
                NewBaseTariffDebt = record.NewBaseTariffDebt,
                NewDecisionTariffDebt = record.NewDecisionTariffDebt,
                NewPenaltyDebt = record.NewPenaltyDebt
            };
        }

        private class DebtInfo
        {
            public decimal BaseTariffDebt { get; set; }
            public decimal DecisionTariffDebt { get; set; }
            public decimal PenaltyDebt { get; set; }
        }
        
        private class Record
        {
            public long Id { get; set; }
            public string PersonalAccountNum { get; set; }
            public PersonalAccountOwnerType OwnerType { get; set; }
            public string OwnerName { get; set; }

            public DateTime OpenDate { get; set; }

            public decimal AreaShare { get; set; }
            public decimal NewAreaShare { get; set; }

            /// <summary>
            /// Идентификатор владельца
            /// </summary>
            public long? AccountOwner { get; set; }
            /// <summary>
            /// Идентификатор контрагента
            /// </summary>
            public long? Contragent { get; set; }

            public string FirstName { get; set; }
            public string Surname { get; set; }
            public string SecondName { get; set; }

            public long RoomId { get; set; }

            public string Inn { get; set; }
            public string Kpp { get; set; }

            public decimal BaseTariffDebt { get; set; }
            public decimal DecisionTariffDebt { get; set; }
            public decimal PenaltyDebt { get; set; }

            public decimal NewBaseTariffDebt { get; set; }
            public decimal NewDecisionTariffDebt { get; set; }
            public decimal NewPenaltyDebt { get; set; }

            [JsonIgnore]
            public decimal NewDebtSum => this.NewBaseTariffDebt + this.NewDecisionTariffDebt + this.NewPenaltyDebt;
        }

        private class PersonalAccountCreateInfo
        {
            public DateTime OpenDate { get; set; }

            public decimal AreaShare { get; set; }

            public PersonalAccountOwner AccountOwner { get; set; }

            public decimal NewBaseTariffDebt { get; set; }
            public decimal NewDecisionTariffDebt { get; set; }
            public decimal NewPenaltyDebt { get; set; }
        }
    }
}