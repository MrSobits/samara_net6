namespace Bars.Gkh.RegOperator.Distribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Distribution.Impl;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainModelServices.Impl.PersonalAccountPayment;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    /// <summary>
    /// Платеж КР
    /// </summary>
    public partial class TransferCrROSPDistribution : AbstractPersonalAccountDistribution
    {
        private readonly IDomainService<AccountPaymentInfoSnapshot> accountSnapshotDomain;

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
        public TransferCrROSPDistribution(
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
            this.accountSnapshotDomain = accountSnapshotDomain;
        }

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.TransferCrROSPDistribution;

        /// <inheritdoc />
        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.TransferCr";

        /// <inheritdoc />
        public override IEnumerable<BasePersonalAccount> GetPersonalAccounts(IQueryable<Transfer> query, ChargePeriod period)
        {
            return this.persaccDomain.GetAll()
                .Fetch(x => x.BaseTariffWallet)
                .Fetch(x => x.DecisionTariffWallet)
                .Fetch(x => x.PenaltyWallet)
                .Fetch(x => x.SocialSupportWallet)
                .Where(y => query.Any(x => x.Owner.Id == y.Id))
                .ToArray()
                .FetchMainWalletInTransfers(period);
        }

        /// <inheritdoc />
        protected override IPersonalAccountPaymentCommand GetCommand()
        {
            return new PersonalAccountTariffPaymentCommand();
        }

        /// <inheritdoc />
        public override IDataResult ListDistributionObjs(BaseParams baseParams)
        {
            var type = baseParams.Params.GetAs<SuspenseAccountDistributionParametersView>("distributionType", ignoreCase: true);

            if (type != SuspenseAccountDistributionParametersView.ByDebt && type != SuspenseAccountDistributionParametersView.ByPaymentDocument)
            {
                return base.ListDistributionObjs(baseParams);
            }

            var persAccQuery = this.ExtractAccountsQuery(baseParams);
            var distributionSum = baseParams.Params.GetAs<decimal?>("distribSum");
            var snapshotId = baseParams.Params.GetAsId("snapshotId");

            var distributeOn = baseParams.Params.GetAs<DistributeOn>("distributeOn");

            var distributable = DistributionHelper.ExtractDistributable(baseParams, 0);

            if (distributionSum == null)
            {
                distributionSum = distributable.RemainSum == 0 ? distributable.Sum : distributable.RemainSum;
            }

            var distribSum = distributionSum.ToDecimal();

            var accounts =
                this.persaccDomain.GetAll()
                    .Where(x => persAccQuery.Any(y => y.Id == x.Id))
                    .Select(x => new
                    {
                        Account = x,
                        PaymentAccountNum = this.calcAccountRoDomain.GetAll()
                            .Where(y => y.Account.TypeAccount == TypeCalcAccount.Special || y.Account.TypeAccount == TypeCalcAccount.Regoperator)
                            .Where(y => y.RealityObject.Id == x.Room.RealityObject.Id)
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

            var summaries =
                this.persAccSummaryDomain.GetAll()
                    .Where(x => persAccQuery.Any(y => y.Id == x.PersonalAccount.Id))
                    .ToList()
                    .GroupBy(x => x.PersonalAccount.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

            var openedPeriod = this.chargePeriodRepo.GetCurrentPeriod();
            foreach (var account in accounts)
            {
                account.Account.SetOpenedPeriodSummary(summaries.Get(account.Account.Id)?.FirstOrDefault(x => x.Period == openedPeriod));
            }

            List<PersAccProxy> proxies;

            switch (type)
            {
                case SuspenseAccountDistributionParametersView.ByDebt:
                    var debts = accounts.Select(
                        x =>
                        {
                            var openedPeriodSummary = x.Account.GetOpenedPeriodSummary();
                            var summary = summaries.Get(x.Account.Id);
                            var debt =
                                summary.SafeSum(
                                    y =>
                                        y.ChargeTariff + y.RecalcByBaseTariff + y.RecalcByDecisionTariff + y.BaseTariffChange + y.DecisionTariffChange
                                        - y.TariffPayment - y.TariffDecisionPayment).RegopRoundDecimal(2);
                            debt = debt >= 0M ? debt : 0M;

                            return new DebtProxy
                            {
                                PersonalAccount = x.Account,
                                TariffDebt = distributeOn != DistributeOn.Penalties ? debt : 0M,
                                PenaltyDebt =
                                    distributeOn != DistributeOn.Charges ? openedPeriodSummary.GetPenaltyDebt() + openedPeriodSummary.PenaltyDebt : 0M,
                                AccountNumber = x.PaymentAccountNum,
                                Id = x.Account.Id
                            };
                        }).ToList();

                    proxies = this.ListByDebt(debts, distribSum, distributeOn, summaries);
                    break;
                case SuspenseAccountDistributionParametersView.ByPaymentDocument:
                    var accInfos =
                        this.accountSnapshotDomain.GetAll()
                            .Where(x => x.Snapshot.Id == snapshotId)
                            .Where(z => persAccQuery.Any(x => x.Id == z.AccountId))
                            .ToArray();

                    var dictStates = persAccQuery.Select(x => new { x.Id, x.State.Name }).ToDictionary(x => x.Id, z => z.Name);

                    proxies = this.ListByPaymentDocument(accInfos, distribSum, dictStates);
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

            switch (distributeOn)
            {
                case DistributeOn.Charges:
                    proxies.ForEach(
                        x =>
                        {
                            var summary = summaries.Get(x.Id);
                            var debt =
                                summary.SafeSum(
                                    y =>
                                        y.ChargeTariff + y.RecalcByBaseTariff + y.RecalcByDecisionTariff + y.BaseTariffChange + y.DecisionTariffChange
                                        - y.TariffPayment - y.TariffDecisionPayment).RegopRoundDecimal(2);
                            x.Debt = debt >= 0M ? debt : 0M;
                        });
                    break;

                case DistributeOn.Penalties:
                    proxies.ForEach(
                        x =>
                        {
                            var summary = summaries.Get(x.Id);
                            var debt = summary.SafeSum(y => y.GetPenaltyDebt()).RegopRoundDecimal(2);
                            x.Debt = debt >= 0M ? debt : 0M;
                        });
                    break;

                default:
                    if (type != SuspenseAccountDistributionParametersView.ByPaymentDocument)
                    {
                        proxies.ForEach(
                            x =>
                            {
                                var summary = summaries.Get(x.Id);
                                var debt = summary.SafeSum(y => y.GetTotalCharge() - y.GetTotalPayment() + y.GetTotalChange()).RegopRoundDecimal(2);
                                x.Debt = debt >= 0M ? debt : 0M;
                            });
                    }

                    break;
            }

            return new ListDataResult(proxies.OrderBy(x => x.Id), proxies.Count);
        }

        /// <inheritdoc />
        public override IDataResult Apply(IDistributionArgs args)
        {
            var distrArgs = args as DistributionByAccountsArgs;
            if (distrArgs == null)
            {
                throw new ArgumentException("args should be convertible to DistributionByAccountsArgs");
            }

            // кэш перерасчетов, когда сажаем оплаты
            this.Container.Resolve<IPersonalAccountRecalcEventManager>().InitCache(new Dictionary<DateTime, BasePersonalAccount[]>
            {
                { distrArgs.Distributable.DateReceipt, distrArgs.DistributionRecords.Select(x => x.PersonalAccount).ToArray() }
            });

            return this.ApplyInternal(args);
        }

        /// <inheritdoc />
        public override IDistributionArgs ExtractArgsFrom(BaseParams baseParams)
        {
            return DistributionByAccountsArgs.FromParams(baseParams);
        }

        private class DebtProxy
        {
            public long Id { get; set; }

            public BasePersonalAccount PersonalAccount { get; set; }

            public string AccountNumber { get; set; }

            public decimal TariffDebt { get; set; }

            public decimal PenaltyDebt { get; set; }
        }
    }
}