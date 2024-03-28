namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainModelServices.Impl.Refund;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;

    using NHibernate.Linq;

    /// <summary>
    /// Распределение средств по типу "Возврат взносов на КР"
    /// </summary>
    public class RefundDistribution : AbstractPersonalAccountDistribution
    {
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
        public RefundDistribution(
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
            : base(
                transferDomain,
                moneyOperationDomain,
                persaccDomain,
                chargePeriodRepo,
                transferRepo,
                importedPaymentDomain,
                detailDomain,
                payAccDmn,
                persAccSummaryDomain,
                accountSnapshotDomain,
                bankProvider,
                calcAccountRoDomain)
        {
        }

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.RefundDistribution;

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.Refund";

        /// <summary>
        /// Проверка применимости распределения к счету НВС
        /// </summary>
        /// <param name="distributable">Распределяемый объект</param>
        /// <returns>Результат проверки</returns>
        public override bool CanApply(IDistributable distributable)
        {
            return distributable.MoneyDirection == MoneyDirection.Outcome;
        }

        /// <summary>
        /// Возврат лицевых счетов по трансферам
        /// </summary>
        /// <param name="query">Запрос трансферов</param>
        /// <param name="period"></param>
        /// <returns>Перечисление аккаунтов</returns>
        public override IEnumerable<BasePersonalAccount> GetPersonalAccounts(IQueryable<Transfer> query, ChargePeriod period)
        {
            return this.persaccDomain.GetAll()
                .Fetch(x => x.BaseTariffWallet)
                .Fetch(x => x.DecisionTariffWallet)
                .Fetch(x => x.PenaltyWallet)
                .Fetch(x => x.SocialSupportWallet)
                .Where(y => query.Any(x => x.Owner.Id == y.Id))
                .ToArray()
                .FetchMainWalletOutTransfers(period);
        }

        /// <summary>
        /// Операция движения денег
        /// </summary>
        /// <returns>Исполняемая команда</returns>
        protected override IPersonalAccountPaymentCommand GetCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="distrArgs">Интерфейс аргументов распределения</param>
        /// <returns>Результат операции</returns>
        public override IDataResult Apply(IDistributionArgs distrArgs)
        {
            var result = new BaseDataResult();
            var args = distrArgs as DistributionByAccountsArgs;
            if (args == null)
            {
                throw new ArgumentException("args should be convertible to DistributionByAccountsArgs");
            }

            if (args.Distributable.MoneyDirection != MoneyDirection.Outcome)
            {
                throw new ArgumentException(@"Запись должна быть с типом Расход", "args");
            }

            var distributable = args.Distributable;
            var records = args.DistributionRecords;

            var operation = this.CreateMoneyOperation(distributable);

            var period = this.chargePeriodRepo.GetCurrentPeriod();
            var paymentDate = distributable.DateReceipt;

            if (period == null)
            {
                throw new ArgumentException("Нет открытого периода");
            }

            if (period.StartDate > paymentDate)
            {
                result.Message = "Оплата будет зафиксирована в текущем периоде, так как в закрытый период оплату посадить невозможно.";
            }

            var command = new PersonalAccountRefundCommand(RefundType.CrPayments);

            var details = new List<DistributionDetail>();

            var container = this.Container;
            var walletDomain = container.ResolveDomain<Wallet>();

            var transfers = new List<Transfer>();
            try
            {
                foreach (var record in records)
                {
                    var wallets = record.PersonalAccount.GetWallets();
                    foreach (var wallet in wallets.Where(x => x.Id == 0))
                    {
                        walletDomain.Save(wallet);
                    }

                    if (record.Sum > 0)
                    {
                        var localTransfers = record.PersonalAccount.ApplyRefund(command,
                            new MoneyStream(args.Distributable, operation, paymentDate, record.Sum));

                        foreach (var localTransfer in localTransfers)
                        {
                            details.Add(new DistributionDetail
                            {
                                Object = record.PersonalAccount.PersonalAccountNum,
                                Sum = localTransfer.Amount
                            });
                        }

                        transfers.AddRange(localTransfers);
                    }
                }
            }
            finally
            {
                container.Release(walletDomain);
            }

            this.moneyOperationDomain.Save(operation);

            transfers.ForEach(this.transferDomain.Save);

            details.GroupBy(x => x.Object)
                .Select(x => new DistributionDetail(args.Distributable.Id, x.Key, x.Sum(z => z.Sum))
                {
                    Destination = "Распределение оплаты",
                    Source = args.Distributable.Source
                })
                .ForEach(this.detailDomain.Save);

            distributable.DistributeState = DistributionState.Distributed;

            return result;
        }

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="distributable">Распределяемый объект</param>
        /// <param name="operation">Отменяемая операция</param>
        public override void Undo(IDistributable distributable, MoneyOperation operation)
        {
            var transferQuery = this.transferRepo.GetByMoneyOperation(operation);

            var period = this.chargePeriodRepo.GetCurrentPeriod();
            var cancelOperation = distributable.CancelOperation(operation, period);

            var command = new PersonalAccountRefundCommand(RefundType.CrPayments);

            var accounts = this.GetPersonalAccounts(transferQuery, operation.Period);
            var transfers = new List<Transfer>();

            foreach (var account in accounts)
            {
                transfers.AddRange(account.UndoRefund(command, cancelOperation, period, distributable.DateReceipt));
            }

            transfers.ForEach(this.transferDomain.Save);
        }

        /// <summary>
        /// Список объектов для распределения средств
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public override IDataResult ListDistributionObjs(BaseParams baseParams)
        {
            var persAccQuery = this.ExtractAccountsQuery(baseParams);
            var distributionSum = baseParams.Params.GetAs<decimal?>("distribSum");

            var type = baseParams.Params.GetAs<SuspenseAccountDistributionParametersView>("distributionType", ignoreCase: true);

            if (type == SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions)
            {
                return base.ListDistributionObjs(baseParams);
            }

            var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);

            if (distributionSum == null)
            {
                distributionSum = distributable.RemainSum == 0 ? distributable.Sum : distributable.RemainSum;
            }

            var distribSum = distributionSum.ToDecimal();

            var accounts = this.persaccDomain.GetAll()
                .Fetch(x => x.AccountOwner)
                .Fetch(x => x.Room)
                .ThenFetch(x => x.RealityObject)
                .Where(x => persAccQuery.Any(y => y.Id == x.Id))
                .LeftJoin(this.payAccDmn.GetAll(),
                    account => account.Room.RealityObject.Id,
                    account => account.RealityObject.Id,
                    (account, paymentAccount) => new BasePersAccProxy
                    {
                        Account = account,
                        PaymentAccountNum = paymentAccount != null ? this.bankProvider.GetBankAccountNumber(paymentAccount.RealityObject) : ""
                    })
                .ToList();

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

                    proxies =
                        Utils.MoneyAndCentDistribution(accounts,
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
                    proxies =
                        Utils.MoneyAndCentDistribution(accounts,
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
                    proxies = RefundDistribution.GetProxiesByDebtByOutcomeMoneyDirection(accounts, distribSum);
                    break;

                default:
                    proxies =
                        Utils.MoneyAndCentDistribution(accounts,
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
                x.Debt = (summary.SafeSum(y => y.GetTotalCharge() - y.GetTotalPayment())).RoundDecimal(2);
            });

            return new ListDataResult(proxies.OrderBy(x => x.Id), proxies.Count);
        }

        /// <summary>
        /// Распределение операции прихода
        /// </summary>
        /// <param name="accounts">Список персональных аккаунтов</param>
        /// <param name="distribSum">Распределяемая сумма</param>
        /// <returns>Информацию о распределениях</returns>
        private static List<PersAccProxy> GetProxiesByDebtByOutcomeMoneyDirection(List<BasePersAccProxy> accounts, decimal distribSum)
        {
            List<PersAccProxy> proxies;
            var debts =
                accounts.Select(x => new
                    {
                        PersonalAccount = x.Account,
                        DebtSum = x.Account.Summaries.SafeSum(y => y.GetBaseTariffDebt()) + x.Account.Summaries.SafeSum(y => y.GetDecisionTariffDebt()),
                        x.PaymentAccountNum
                    })
                    .ToList();

            var debtsNeedsToDistibute = debts.Where(x => x.DebtSum < 0).ToList();

            //сумма будет отрицательной
            var debtsSum = debtsNeedsToDistibute.Sum(x => x.DebtSum);

            //так как задолженность у ЛС отрицательная у тех, кому необходим возврат средств, то суммируем по модулю
            if (debtsNeedsToDistibute.SafeSum(x => Math.Abs(x.DebtSum)) >= distribSum)
            {
                proxies =
                    Utils.MoneyAndCentDistribution(debts,
                        x => x.DebtSum >= 0
                            ? 0M
                            : (distribSum * x.DebtSum / debtsSum).RoundDecimal(2),
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
                proxies =
                    Utils.MoneyAndCentDistribution(debts,
                        x => x.DebtSum >= 0
                            ? 0M
                            : -x.DebtSum.RoundDecimal(2),
                        Math.Abs(debtsSum),
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
            return proxies;
        }
    }
}