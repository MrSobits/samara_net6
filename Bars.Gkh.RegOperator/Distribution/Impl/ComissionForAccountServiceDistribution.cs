namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    using NHibernate.Linq;

    using Utils = Bars.Gkh.RegOperator.Distribution.Utils;

    /// <summary>
    /// Комиссия за ведение счета кредитной организацией
    /// </summary>
    public class ComissionForAccountServiceDistribution : BaseDistribution
    {
        private readonly IDomainService<RealityObjectPaymentAccount> ropayAccDomain;
        private readonly IDomainService<RealityObjectTransfer> transferDomain;
        private readonly IDomainService<MoneyOperation> moneyOperDomain;
        private readonly ITransferRepository<RealityObjectTransfer> transferRepo;
        private readonly IChargePeriodRepository periodRepo;
        private readonly IDomainService<CalcAccountRealityObject> calcAccountRoDomain;
        private readonly IDomainService<ObjectCr> objectCrDomain;
        private readonly IDomainService<DistributionDetail> distributionDetailDomain;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ropayAccDomain">
        /// DomainService <see cref="RealityObjectPaymentAccount" />
        /// </param>
        /// <param name="transferDomain">
        /// Домен-сервис <see cref="Transfer" />
        /// </param>
        /// <param name="moneyOperDomain">
        /// Домен-сервис <see cref="MoneyOperation" />
        /// </param>
        /// <param name="transferRepo">
        /// Репозиторий трансферов
        /// </param>
        /// <param name="periodRepo">
        /// Репозиторий периодов
        /// </param>
        /// <param name="calcAccountRoDomain">
        /// Домен-сервис <see cref="CalcAccountRealityObject" />
        /// </param>
        /// <param name="distributionDetailDomain">
        /// Домен-сервис <see cref="DistributionDetail" />
        /// </param>
        /// <param name="objectCrDomain">
        /// Домен-сервис <see cref="ObjectCr" />
        /// </param>
        public ComissionForAccountServiceDistribution(
            IDomainService<RealityObjectPaymentAccount> ropayAccDomain,
            IDomainService<RealityObjectTransfer> transferDomain,
            IDomainService<MoneyOperation> moneyOperDomain,
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IChargePeriodRepository periodRepo,
            IDomainService<CalcAccountRealityObject> calcAccountRoDomain,
            IDomainService<DistributionDetail> distributionDetailDomain,
            IDomainService<ObjectCr> objectCrDomain)
        {
            this.ropayAccDomain = ropayAccDomain;
            this.transferRepo = transferRepo;
            this.periodRepo = periodRepo;
            this.transferDomain = transferDomain;
            this.moneyOperDomain = moneyOperDomain;
            this.calcAccountRoDomain = calcAccountRoDomain;
            this.distributionDetailDomain = distributionDetailDomain;
            this.objectCrDomain = objectCrDomain;
        }

        /// <inheritdoc />
        public override string Route => "realtyaccdistribution";

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.ComissionForAccountServiceDistribution;

        /// <inheritdoc />
        public override string PermissionId { get; } = "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.ComissionForAccountService";

        /// <inheritdoc />
        public override bool CanApply(IDistributable suspenseAccount)
        {
            return suspenseAccount.MoneyDirection == MoneyDirection.Outcome;
        }

        /// <inheritdoc />
        public override void Undo(IDistributable suspenseAccount, MoneyOperation operation)
        {
            var transferQuery = this.transferRepo.GetByMoneyOperation(operation);

            var accounts = this.GetPaymentAccounts(transferQuery);

            var transfers = transferQuery
                .Where(x => !x.Operation.IsCancelled)
                .ToDictionary(x => x.SourceGuid);

            var period = this.periodRepo.GetCurrentPeriod();

            var moneyOperation = suspenseAccount.CancelOperation(operation, period);

            List<Transfer> transferList = new List<Transfer>();

            foreach (var account in accounts)
            {
                var applyBaseTariffTransfer = transfers.Get(account.BaseTariffPaymentWallet.WalletGuid);

                if (applyBaseTariffTransfer != null)
                {
                    var undoTransfer = account.StoreMoney(
                        account.BaseTariffPaymentWallet,
                        new MoneyStream(
                            suspenseAccount,
                            moneyOperation,
                            suspenseAccount.DateReceipt,
                            applyBaseTariffTransfer.Amount)
                        {
                            Description =
                                "Отмена комиссии за ведение счета кредитной организацией(возвращено в поступления по базовому тарифу)"
                        },
                        false);

                    transferList.Add(undoTransfer);
                }

                var applyDecisionTariffTransfer = transfers.Get(account.DecisionPaymentWallet.WalletGuid);

                if (applyDecisionTariffTransfer != null)
                {
                    var undoTransfer = account.StoreMoney(
                        account.DecisionPaymentWallet,
                        new MoneyStream(
                            suspenseAccount,
                            moneyOperation,
                            suspenseAccount.DateReceipt,
                            applyDecisionTariffTransfer.Amount)
                        {
                            Description =
                                "Отмена комиссии за ведение счета кредитной организацией(возвращено в поступления по тарифу решения)"
                        },
                        false);

                    transferList.Add(undoTransfer);
                }

                var applyPenaltyTransfer = transfers.Get(account.PenaltyPaymentWallet.WalletGuid);

                if (applyPenaltyTransfer != null)
                {
                    var undoTransfer = account.StoreMoney(
                        account.PenaltyPaymentWallet,
                        new MoneyStream(
                            suspenseAccount,
                            moneyOperation,
                            suspenseAccount.DateReceipt,
                            applyPenaltyTransfer.Amount)
                        {
                            Description =
                                "Отмена комиссии за ведение счета кредитной организацией(возвращено в поступления по пеням)"
                        },
                        false);

                    transferList.Add(undoTransfer);
                }
            }

            if (transferList.Count > 0)
            {
                this.moneyOperDomain.Save(moneyOperation);
                transferList.ForEach(this.transferDomain.Save);
            }
        }

        /// <inheritdoc />
        public override IDataResult Apply(IDistributionArgs args)
        {
            var distrArgs = args as DistributionByRealtyAccountArgs;
            if (distrArgs == null)
            {
                throw new ArgumentException("args should be convertible to DistributionByRealtyArgs");
            }

            var suspAccount = distrArgs.Distributable;
            var period = this.periodRepo.GetCurrentPeriod();

            var moneyOperation = this.CreateMoneyOperation(suspAccount);
            moneyOperation.Reason = "Комиссия за ведение счета кредитной организацией";
            moneyOperation.OperationDate = suspAccount.DateReceipt;
            moneyOperation.Amount = suspAccount.Sum;

            var roWithoutNeedMoney =
                distrArgs.DistributionRecords
                    .Where(x => x.RealtyAccount.BaseTariffPaymentWallet.Balance
                        + x.RealtyAccount.DecisionPaymentWallet.Balance
                        + x.RealtyAccount.PenaltyPaymentWallet.Balance < x.Sum)
                    .Select(x => x.RealtyAccount.RealityObject.Address)
                    .ToList();

            if (roWithoutNeedMoney.Count > 0)
            {
                throw new ArgumentException(
                    "В следующих домах не хватает средств: {0}".FormatUsing(roWithoutNeedMoney.AggregateWithSeparator(x => x, ", ")));
            }

            var transfers = new List<Transfer>();
            var details = new List<DistributionDetail>();

            foreach (var record in distrArgs.DistributionRecords)
            {
                var sum = record.Sum;

                var tempSum = Math.Min(sum, record.RealtyAccount.BaseTariffPaymentWallet.Balance);

                if (tempSum > 0)
                {
                    var moneyStream = new MoneyStream(suspAccount, moneyOperation, suspAccount.DateReceipt, tempSum)
                    {
                        Description = "Комиссия за ведение счета кредитной организацией(снято из поступлений по базовому тарифу)"
                    };

                    sum -= tempSum;

                    transfers.Add(record.RealtyAccount.UndoPayment(record.RealtyAccount.BaseTariffPaymentWallet, moneyStream, false));
                    details.Add(new DistributionDetail(distrArgs.Distributable.Id, record.RealtyAccount.RealityObject.Address, moneyStream.Amount)
                    {
                        Source = distrArgs.Distributable.Source,
                        Destination = "Распределение оплаты"
                    });
                }

                tempSum = Math.Min(sum, record.RealtyAccount.DecisionPaymentWallet.Balance);

                if (tempSum > 0)
                {
                    var moneyStream = new MoneyStream(suspAccount, moneyOperation, suspAccount.DateReceipt, tempSum)
                    {
                        Description = "Комиссия за ведение счета кредитной организацией(снято из поступлений по тарифу решений)"
                    };

                    sum -= tempSum;

                    transfers.Add(record.RealtyAccount.UndoPayment(record.RealtyAccount.DecisionPaymentWallet, moneyStream, false));
                    details.Add(new DistributionDetail(distrArgs.Distributable.Id, record.RealtyAccount.RealityObject.Address, moneyStream.Amount)
                    {
                        Source = distrArgs.Distributable.Source,
                        Destination = "Распределение оплаты"
                    });
                }

                if (sum > 0)
                {
                    var moneyStream = new MoneyStream(suspAccount, moneyOperation, suspAccount.DateReceipt, sum)
                    {
                        Description = "Комиссия за ведение счета кредитной организацией(снято из поступлений по пеням)"
                    };

                    transfers.Add(record.RealtyAccount.UndoPayment(record.RealtyAccount.PenaltyPaymentWallet, moneyStream, false));
                    details.Add(new DistributionDetail(distrArgs.Distributable.Id, record.RealtyAccount.RealityObject.Address, moneyStream.Amount)
                    {
                        Source = distrArgs.Distributable.Source,
                        Destination = "Распределение оплаты"
                    });
                }
            }

            if (transfers.Count > 0)
            {
                details.ForEach(this.distributionDetailDomain.Save);
                this.moneyOperDomain.Save(moneyOperation);
                transfers.ForEach(this.transferDomain.Save);
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public override IDistributionArgs ExtractArgsFrom(BaseParams baseParams)
        {
            return DistributionByRealtyAccountArgs.FromParams(baseParams);
        }

        /// <inheritdoc />
        public override IDistributionArgs ExtractArgsFromMany(BaseParams baseParams, int counter, decimal thisOneDistribSum)
        {
            return DistributionByRealtyAccountArgs.FromManyParams(baseParams, counter, thisOneDistribSum);
        }

        /// <inheritdoc />
        public override IDataResult ListDistributionObjs(BaseParams baseParams)
        {
            var distributionSum = baseParams.Params.GetAs<decimal?>("distribSum");

            var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);

            if (distributionSum == null)
            {
                distributionSum = distributable.RemainSum == 0 ? distributable.Sum : distributable.RemainSum;
            }

            var distribSum = distributionSum.ToDecimal();

            var type = baseParams.Params.GetAs<SuspenseAccountDistributionParametersView>("distributionType");

            RealityObjectPaymentAccount[] accounts = null;
            RoPaymentAccountProxy[] accountsProxies = null;

            if (type == SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions)
            {
                accountsProxies = this.GetAccountProxies(baseParams);
            }
            else
            {
                accounts = this.GetAccounts(baseParams);
            }

            List<Proxy> proxies;

            switch (type)
            {
                case SuspenseAccountDistributionParametersView.ProportionalArea:
                    var sumArea = accounts?.Sum(x => x.RealityObject.AreaLiving) ?? 0m;

                    if (sumArea == 0m)
                    {
                        return new BaseDataResult(false, "Невозможно выполнить распределение по площадям, сумма площадей равна нулю");
                    }

                    proxies =
                        Utils.MoneyAndCentDistribution(accounts,
                            x => x.RealityObject.AreaLiving ?? 0m / sumArea * distribSum,
                            distribSum,
                            (acc, sum) => new Proxy(acc, sum),
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
                    proxies =
                        Utils.MoneyAndCentDistribution(
                            accounts,
                            x => distribSum / accounts.Length,
                            distribSum,
                            (acc, sum) => new Proxy(acc, sum),
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

                    if (accountsProxies.Length == 0)
                    {
                        return new BaseDataResult(false, "Необходимо выбрать хотя бы 1 запись для распределения");
                    }

                    var inWalletGuids = accountsProxies.SelectMany(
                            x => new[]
                            {
                                x.RoPaymentAccount.BaseTariffPaymentWallet.WalletGuid,
                                x.RoPaymentAccount.DecisionPaymentWallet.WalletGuid,
                                x.RoPaymentAccount.PenaltyPaymentWallet.WalletGuid,
                                x.RoPaymentAccount.SocialSupportWallet.WalletGuid,
                                x.RoPaymentAccount.AccumulatedFundWallet.WalletGuid,
                                x.RoPaymentAccount.PreviosWorkPaymentWallet.WalletGuid
                            })
                        .ToArray();

                    var outWalletGuids = accountsProxies.SelectMany(
                            x => new[]
                            {
                                x.RoPaymentAccount.BaseTariffPaymentWallet.WalletGuid,
                                x.RoPaymentAccount.DecisionPaymentWallet.WalletGuid,
                                x.RoPaymentAccount.PenaltyPaymentWallet.WalletGuid,
                                x.RoPaymentAccount.SocialSupportWallet.WalletGuid,
                                x.RoPaymentAccount.AccumulatedFundWallet.WalletGuid,
                                x.RoPaymentAccount.RentWallet.WalletGuid
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

                    var outTransfers = this.transferDomain.GetAll()
                        .Fetch(x => x.Operation)
                        .Where(x => outWalletGuids.Contains(x.SourceGuid))
                        .Where(x => x.Operation.CanceledOperation == null)
                        .Where(x => x.PaymentDate.Date >= periodStartDate && x.PaymentDate.Date <= periodEndDate)
                        .Where(x => x.Amount != 0)
                        .GroupBy(x => x.SourceGuid)
                        .Select(x => new { x.Key, Amount = x.Sum(z => z.Amount) })
                        .Where(x => x.Amount != 0)
                        .ToDictionary(x => x.Key, x => x.Amount);

                    var sumPeriodAccumulation = 0m;
                    foreach (var roAccount in accountsProxies)
                    {
                        var paymentsSum = this.GetTransfersSum(
                            inTransfers,
                            roAccount.RoPaymentAccount.BaseTariffPaymentWallet.WalletGuid,
                            roAccount.RoPaymentAccount.DecisionPaymentWallet.WalletGuid,
                            roAccount.RoPaymentAccount.PenaltyPaymentWallet.WalletGuid,
                            roAccount.RoPaymentAccount.SocialSupportWallet.WalletGuid,
                            roAccount.RoPaymentAccount.AccumulatedFundWallet.WalletGuid,
                            roAccount.RoPaymentAccount.PreviosWorkPaymentWallet.WalletGuid);

                        var chargeSum = this.GetTransfersSum(
                            outTransfers,
                            roAccount.RoPaymentAccount.BaseTariffPaymentWallet.WalletGuid,
                            roAccount.RoPaymentAccount.DecisionPaymentWallet.WalletGuid,
                            roAccount.RoPaymentAccount.PenaltyPaymentWallet.WalletGuid,
                            roAccount.RoPaymentAccount.SocialSupportWallet.WalletGuid,
                            roAccount.RoPaymentAccount.AccumulatedFundWallet.WalletGuid,
                            roAccount.RoPaymentAccount.RentWallet.WalletGuid);

                        var periodAccumulation = paymentsSum - chargeSum;
                        if (periodAccumulation > 0)
                        {
                            roAccount.PeriodAccumulation = periodAccumulation;
                            sumPeriodAccumulation += roAccount.PeriodAccumulation;
                        }
                    }

                    proxies = Utils.MoneyAndCentDistribution(
                        accountsProxies,
                        x => sumPeriodAccumulation > 0 ? (x.PeriodAccumulation * (distribSum / sumPeriodAccumulation)) : 0,
                        distribSum,
                        (acc, sum) =>
                            new Proxy(
                                acc.RoPaymentAccount,
                                sum,
                                acc.PeriodAccumulation),
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
                    proxies =
                        Utils.MoneyAndCentDistribution(
                            accounts,
                            x => 0m,
                            0m,
                            (acc, sum) => new Proxy(acc, sum),
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

            return new ListDataResult(proxies, proxies.Count);
        }

        /// <summary>
        /// Получить счета оплат домов, связанных с трансферами
        /// </summary>
        /// <param name="query">Поздапрос трансферов</param>
        /// <returns>Счета оплат домов</returns>
        private IEnumerable<RealityObjectPaymentAccount> GetPaymentAccounts(IQueryable<Transfer> query)
        {
            return this.ropayAccDomain.GetAll()
                .Where(y => query.Any(x => x.SourceGuid == y.BankPercentWallet.WalletGuid
                    || x.SourceGuid == y.DecisionPaymentWallet.WalletGuid
                    || x.SourceGuid == y.PenaltyPaymentWallet.WalletGuid))
                .ToArray();
        }

        private RealityObjectPaymentAccount[] GetAccounts(BaseParams baseParams)
        {
            RealityObjectPaymentAccount[] accounts;
            if (baseParams.Params.GetAs<bool>("distributeUsingFilters"))
            {
                var container = ApplicationContext.Current.Container;
                var loadParam = baseParams.GetLoadParam();

                var programCrId = baseParams.Params.GetAs<long>("programCr");
                var crFundTypes = baseParams.Params.GetAs<int[]>("crFundTypes");
                var bankAccNum = baseParams.Params.GetAs<bool>("bankAccNum");

                long[] roIds = null;
                var roIdsStr = baseParams.Params.GetAs<string>("roIds");
                if (roIdsStr != null && !roIdsStr.Contains("All"))
                {
                    roIds = roIdsStr.ToLongArray();
                }

                var roIdsByProgramCr = this.objectCrDomain.GetAll()
                    .Where(x => x.ProgramCr.Id == programCrId)
                    .Select(x => x.RealityObject.Id)
                    .ToArray();

                accounts = this.ropayAccDomain.GetAll()
                    .Select(
                        x => new
                        {
                            RoPaymentAccount = x,
                            Municipality = x.RealityObject.Municipality.Name,
                            RealityObject = x.RealityObject.Address,
                            AccountNum = x.AccountNumber,
                            RealityId = x.RealityObject.Id,
                            CrFundType = (int) x.RealityObject.AccountFormationVariant.Value,
                            BankAccountNumber = this.calcAccountRoDomain.GetAll()
                                .Where(y => y.Account.TypeAccount == TypeCalcAccount.Special || y.Account.TypeAccount == TypeCalcAccount.Regoperator)
                                .Where(y => y.RealityObject.Id == x.RealityObject.Id)
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
                    .WhereIf(programCrId != 0, x => roIdsByProgramCr.Contains(x.RealityId))
                    .WhereIf(crFundTypes.IsNotEmpty(), x => crFundTypes.Contains(x.CrFundType))
                    .WhereIf(roIds.IsNotEmpty(), x => roIds.Contains(x.RealityId))
                    .WhereIf(roIds.IsEmpty() && bankAccNum, x => x.BankAccountNumber != null && x.BankAccountNumber != string.Empty)
                    .Filter(loadParam, container)
                    .Select(x => x.RoPaymentAccount)
                    .ToArray();
            }
            else
            {
                var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
                accounts = this.ropayAccDomain.GetAll().Where(x => ids.Contains(x.Id)).ToArray();
            }

            return accounts;
        }

        private RoPaymentAccountProxy[] GetAccountProxies(BaseParams baseParams)
        {
            RoPaymentAccountProxy[] accounts;
            if (baseParams.Params.GetAs<bool>("distributeUsingFilters"))
            {
                var container = ApplicationContext.Current.Container;
                var loadParam = baseParams.GetLoadParam();

                var programCrId = baseParams.Params.GetAs<long>("programCr");
                var crFundTypes = baseParams.Params.GetAs<int[]>("crFundTypes");
                var bankAccNum = baseParams.Params.GetAs<bool>("bankAccNum");

                long[] roIds = null;
                var roIdsStr = baseParams.Params.GetAs<string>("roIds");
                if (roIdsStr != null && !roIdsStr.Contains("All"))
                {
                    roIds = roIdsStr.ToLongArray();
                }

                var roIdsByProgramCr = this.objectCrDomain.GetAll()
                    .Where(x => x.ProgramCr.Id == programCrId)
                    .Select(x => x.RealityObject.Id)
                    .ToArray();

                accounts = this.ropayAccDomain.GetAll()
                    .Fetch(x => x.BaseTariffPaymentWallet)
                    .Fetch(x => x.DecisionPaymentWallet)
                    .Fetch(x => x.PenaltyPaymentWallet)
                    .Fetch(x => x.SocialSupportWallet)
                    .Fetch(x => x.AccumulatedFundWallet)
                    .Fetch(x => x.RentWallet)
                    .Fetch(x => x.RealityObject)
                    .ThenFetch(x => x.Municipality)
                    .Select(
                        x => new
                        {
                            RoPaymentAccount = x,
                            Municipality = x.RealityObject.Municipality.Name,
                            RealityObject = x.RealityObject.Address,
                            AccountNum = x.AccountNumber,
                            RealityId = x.RealityObject.Id,
                            CrFundType = (int) x.RealityObject.AccountFormationVariant.Value,
                            BankAccountNumber = this.calcAccountRoDomain.GetAll()
                                .Where(
                                    y =>
                                        (y.Account.TypeAccount == TypeCalcAccount.Special && ((SpecialCalcAccount)y.Account).IsActive)
                                        || y.Account.TypeAccount == TypeCalcAccount.Regoperator)
                                .Where(y => y.RealityObject.Id == x.RealityObject.Id)
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
                    .WhereIf(programCrId != 0, x => roIdsByProgramCr.Contains(x.RealityId))
                    .WhereIf(crFundTypes.IsNotEmpty(), x => crFundTypes.Contains(x.CrFundType))
                    .WhereIf(roIds.IsNotEmpty(), x => roIds.Contains(x.RealityId))
                    .WhereIf(roIds.IsEmpty() && bankAccNum, x => x.BankAccountNumber != null && x.BankAccountNumber != string.Empty)
                    .Filter(loadParam, container)
                    .Select(
                        x => new RoPaymentAccountProxy
                        {
                            RoPaymentAccount = x.RoPaymentAccount,
                            PeriodAccumulation = 0m
                        })
                    .ToArray();
            }
            else
            {
                var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

                accounts = this.ropayAccDomain.GetAll()
                    .Fetch(x => x.BaseTariffPaymentWallet)
                    .Fetch(x => x.DecisionPaymentWallet)
                    .Fetch(x => x.PenaltyPaymentWallet)
                    .Fetch(x => x.SocialSupportWallet)
                    .Fetch(x => x.AccumulatedFundWallet)
                    .Fetch(x => x.RentWallet)
                    .Fetch(x => x.RealityObject)
                    .ThenFetch(x => x.Municipality)
                    .Where(x => ids.Contains(x.Id)).Select(
                        x => new RoPaymentAccountProxy
                        {
                            RoPaymentAccount = x,
                            PeriodAccumulation = 0m
                        }).ToArray();
            }

            return accounts;
        }

        private decimal GetTransfersSum(Dictionary<string, decimal> transfers, params string[] walletIds)
        {
            return walletIds.Sum(x => transfers.Get(x));
        }

        private class Proxy
        {
            public Proxy(RealityObjectPaymentAccount account, decimal sum, decimal periodAccumulation = 0m)
            {
                this.RealtyAccountId = account.Id;
                this.Municipality = account.RealityObject.Municipality.Name;
                this.RealityObject = account.RealityObject.Address;
                this.Sum = sum;
                this.PeriodAccumulation = periodAccumulation;
            }

            public string Municipality { get; set; }

            public long RealtyAccountId { get; set; }

            public string RealityObject { get; set; }

            public decimal Sum { get; set; }

            /// <summary>
            /// Накопления за период
            /// </summary>
            public decimal PeriodAccumulation { get; set; }
        }

        private class RoPaymentAccountProxy
        {
            public RealityObjectPaymentAccount RoPaymentAccount { get; set; }

            public decimal PeriodAccumulation { get; set; }
        }
    }
}