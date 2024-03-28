namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.Analytics.Utils;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.GkhCr.Entities;

    using NHibernate.Linq;

    /// <summary>
    /// Абстрактный возврат на счета оплат жилого дома
    /// </summary>
    public abstract class AbstractRealtyAccountRefundDistribution : BaseDistribution
    {
        protected readonly IDomainService<RealityObjectPaymentAccount> RopayAccDomain;
        private readonly IDomainService<RealityObjectTransfer> transferDomain;
        private readonly ITransferRepository<RealityObjectTransfer> transferRepo;
        private readonly IChargePeriodRepository periodRepo;
        private readonly IDomainService<DistributionDetail> detailDomain;
        private readonly IDomainService<MoneyOperation> moneyOperationDomain;
        private readonly IDomainService<BasePersonalAccount> personalAccountDomain;
        private readonly IDomainService<CalcAccountRealityObject> calcAccountRoDomain;
        private readonly IDomainService<ObjectCr> objectCrDomain;

        /// <summary>
        /// Конструктор
        /// </summary>
        protected AbstractRealtyAccountRefundDistribution(
            IDomainService<RealityObjectPaymentAccount> ropayAccDomain,
            IDomainService<RealityObjectTransfer> transferDomain,
            ITransferRepository<RealityObjectTransfer> transferRepo,
            IChargePeriodRepository periodRepo,
            IDomainService<DistributionDetail> detailDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<BasePersonalAccount> personalAccountDomain,
            IDomainService<CalcAccountRealityObject> calcAccountRoDomain,
            IDomainService<ObjectCr> objectCrDomain)
        {
            this.RopayAccDomain = ropayAccDomain;
            this.transferRepo = transferRepo;
            this.periodRepo = periodRepo;
            this.detailDomain = detailDomain;
            this.transferDomain = transferDomain;
            this.moneyOperationDomain = moneyOperationDomain;
            this.personalAccountDomain = personalAccountDomain;
            this.calcAccountRoDomain = calcAccountRoDomain;
            this.objectCrDomain = objectCrDomain;
        }

        /// <summary>
        /// Роут клиентского контроллера
        /// </summary>
        public override string Route => "realtyaccdistribution";

        /// <summary>
        /// Проверка применимости распределения к счету НВС
        /// </summary>
        /// <param name="suspenseAccount"></param>
        /// <returns></returns>
        public override bool CanApply(IDistributable suspenseAccount)
        {
            return suspenseAccount.MoneyDirection == MoneyDirection.Outcome;
        }

        /// <summary>
        /// Отменить возврат
        /// </summary>
        /// <param name="distributable"></param>
        /// <param name="operation">Отменяемая операция</param>
        public override void Undo(IDistributable distributable, MoneyOperation operation)
        {
            var transferQuery = this.transferRepo.GetByMoneyOperation(operation);

            var accounts = this.GetPaymentAccounts(transferQuery).ToList();

            var period = this.periodRepo.GetCurrentPeriod();

            var moneyOperation = operation.Cancel(period);

            this.moneyOperationDomain.Save(moneyOperation);

            var transfers = transferQuery
                .Where(x => !x.Operation.IsCancelled)
                .ToDictionary(x => x.SourceGuid);

            foreach (var account in accounts)
            {
                var wallet = this.GetWallet(account);

                var applyTransfer = transfers.Get(wallet.WalletGuid);

                var moneyStream = new MoneyStream(
                    distributable,
                    moneyOperation,
                    distributable.DateReceipt,
                    applyTransfer.Amount)
                {
                    Description = applyTransfer.Reason.IsEmpty()
                        ? "Отмена распределения средств"
                        : "Отмена распределения ({0})".FormatUsing(applyTransfer.Reason),
                    OriginalTransfer = applyTransfer,
                    IsAffect = true
                };

                var newTransfer = account.StoreMoney(wallet, moneyStream, false);
                this.transferDomain.Save(newTransfer);
            }
        }

        public override IDataResult Apply(IDistributionArgs args)
        {
            var distrArgs = args as DistributionByRealtyAccountArgs;
            if (distrArgs == null)
            {
                throw new ArgumentException("args should be convertible to DistributionByRealtyArgs");
            }
            this.Apply(distrArgs);

            return new BaseDataResult();
        }

        public override IDistributionArgs ExtractArgsFrom(BaseParams baseParams)
        {
            return DistributionByRealtyAccountArgs.FromParams(baseParams);
        }

        public override IDistributionArgs ExtractArgsFromMany(BaseParams baseParams, int counter, decimal thisOneDistribSum)
        {
            return DistributionByRealtyAccountArgs.FromManyParams(baseParams, counter, thisOneDistribSum);
        }

        public override IDataResult ListDistributionObjs(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
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
                    var sumArea = accounts.Sum(x => x.RealityObject.AreaLiving) ?? 0m;

                    if (sumArea == 0m)
                    {
                        return new BaseDataResult(
                            false,
                            "Невозможно выполнить распределение по площадям, сумма площадей равна нулю");
                    }

                    proxies =
                        Utils.MoneyAndCentDistribution(
                            accounts,
                            x => x.RealityObject.AreaLiving.ToDecimal() / sumArea * distribSum,
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

                case SuspenseAccountDistributionParametersView.ByDebt:
                    var roAccountService = ApplicationContext.Current.Container.Resolve<IRealityObjectAccountService>();

                    var debts = accounts
                        .Select(
                            x => new
                            {
                                Account = x,
                                roAccountService.GetLastClosedOperation(x.RealityObject).SaldoOut
                            })
                        .ToList();

                    ApplicationContext.Current.Container.Release(roAccountService);

                    if (debts.SafeSum(x => x.SaldoOut) >= distribSum)
                    {
                        proxies =
                            Utils.MoneyAndCentDistribution(
                                debts,
                                x => x.SaldoOut <= 0
                                    ? 0M
                                    : distribSum * x.SaldoOut /
                                    debts.Where(y => y.SaldoOut > 0).Sum(y => y.SaldoOut),
                                distribSum,
                                (debt, sum) => new Proxy(debt.Account, sum),
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
                        var totalArea = accounts.Sum(x => x.RealityObject.AreaLiving) ?? 0m;
                        var debtsByAccountId = debts
                            .GroupBy(x => x.Account.Id)
                            .ToDictionary(x => x.Key, x => x.Single().SaldoOut);

                        proxies =
                            Utils.MoneyAndCentDistribution(
                                accounts,
                                x => x.RealityObject.AreaLiving.ToDecimal() / totalArea * extraMoney + debtsByAccountId.Get(x.Id),
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
                            roAccount.RoPaymentAccount.PreviosWorkPaymentWallet.WalletGuid
                        );

                        var chargeSum = this.GetTransfersSum(
                            outTransfers,
                            roAccount.RoPaymentAccount.BaseTariffPaymentWallet.WalletGuid,
                            roAccount.RoPaymentAccount.DecisionPaymentWallet.WalletGuid,
                            roAccount.RoPaymentAccount.PenaltyPaymentWallet.WalletGuid,
                            roAccount.RoPaymentAccount.SocialSupportWallet.WalletGuid,
                            roAccount.RoPaymentAccount.AccumulatedFundWallet.WalletGuid,
                            roAccount.RoPaymentAccount.RentWallet.WalletGuid
                        );

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
                        }
                    );

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
        /// Получить кошелёк
        /// </summary>
        protected abstract Wallet GetWallet(RealityObjectPaymentAccount realityObjectPaymentAccount);

        /// <summary>
        /// Применить возврат
        /// </summary>
        /// <param name="args"></param>
        private void Apply(DistributionByRealtyAccountArgs args)
        {
            var distributable = args.Distributable;

            var moneyOperation = this.CreateMoneyOperation(distributable);

            foreach (var record in args.DistributionRecords.Where(x => x.Sum > 0))
            {
                var wallet = this.GetWallet(record.RealtyAccount);
                if (record.Sum > wallet.Balance)
                {
                    throw new Exception("Недостаточно денег для возврата");
                }

                var moneyStream = new MoneyStream(
                    distributable,
                    moneyOperation,
                    distributable.DateReceipt,
                    args.SumDistribution != default ? args.SumDistribution / args.DistributionRecords.Where(x => x.Sum > 0).Count() : record.Sum)
                {
                    IsAffect = true,
                    Description = this.Name
                };

                var newTransfer = record.RealtyAccount.TakeMoney(wallet, moneyStream);
                this.transferDomain.Save(newTransfer);

                var detail = new DistributionDetail(args.Distributable.Id, record.RealtyAccount.RealityObject.Address, newTransfer.Amount)
                {
                    Source = args.Distributable.Source,
                    Destination = "Распределение оплаты"
                };
                this.detailDomain.Save(detail);
            }
        }

        /// <summary>
        /// Получить счета оплат домов, связанных с трансферами
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected abstract IEnumerable<RealityObjectPaymentAccount> GetPaymentAccounts(IQueryable<Transfer> query);

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

                accounts = this.RopayAccDomain.GetAll()
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
                    .WhereIf(roIds.IsEmpty() && bankAccNum, x => x.BankAccountNumber != null && x.BankAccountNumber != "")
                    .Filter(loadParam, container)
                    .Select(x => x.RoPaymentAccount)
                    .ToArray();
            }
            else
            {
                var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
                accounts = this.RopayAccDomain.GetAll().Where(x => ids.Contains(x.Id)).ToArray();
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

                accounts = this.RopayAccDomain.GetAll()
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
                                .Where(y => (y.Account.TypeAccount == TypeCalcAccount.Special && ((SpecialCalcAccount)y.Account).IsActive)
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
                    .WhereIf(roIds.IsEmpty() && bankAccNum, x => x.BankAccountNumber != null && x.BankAccountNumber != "")
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

                accounts = this.RopayAccDomain.GetAll()
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
                this.RealityObject = account.RealityObject.Address;
                this.Sum = sum;
                this.Municipality = account.RealityObject.Municipality.Name;
                this.PeriodAccumulation = periodAccumulation;
            }

            public long RealtyAccountId { get; set; }

            public string RealityObject { get; set; }

            public decimal Sum { get; set; }

            public string Municipality { get; set; }

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