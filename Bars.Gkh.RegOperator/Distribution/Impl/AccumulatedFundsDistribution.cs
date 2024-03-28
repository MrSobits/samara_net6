namespace Bars.Gkh.RegOperator.Distribution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Distribution.Args;
    using Bars.Gkh.RegOperator.Distribution.Impl;
    using Bars.Gkh.RegOperator.Domain.Repository.Transfers;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainModelServices.Impl.PersonalAccountPayment;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;

    using NHibernate.Linq;

    /// <summary>
    /// Ранее накопленные средства
    /// </summary>
    public class AccumulatedFundsDistribution : AbstractPersonalAccountDistribution
    {
        private readonly IUltimateDecisionService decisionService;

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
        /// <param name="decisionService"></param>
        /// <param name="accountSnapshotDomain">Домен-сервис "Данные для документа на оплату по ЛС"</param>
        /// <param name="bankProvider">Интерфейс для получения данных банка</param>
        /// <param name="calcAccountRoDomain">Домен-сервис "Жилой дом расчетного счета"</param>
        public AccumulatedFundsDistribution(
            IDomainService<PersonalAccountPaymentTransfer> transferDomain,
            IDomainService<MoneyOperation> moneyOperationDomain,
            IDomainService<BasePersonalAccount> persaccDomain,
            IChargePeriodRepository chargePeriodRepo,
            ITransferRepository<PersonalAccountPaymentTransfer> transferRepo,
            IDomainService<ImportedPayment> importedPaymentDomain,
            IDomainService<DistributionDetail> detailDomain,
            IDomainService<RealityObjectPaymentAccount> payAccDmn,
            IDomainService<PersonalAccountPeriodSummary> persAccSummaryDomain,
            IUltimateDecisionService decisionService,
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
            this.decisionService = decisionService;
        }

        /// <summary>
        /// Код распределения
        /// </summary>
        public override DistributionCode DistributionCode => DistributionCode.AccumulatedFundsDistribution;

        /// <inheritdoc />
        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.AccumulatedFunds";

        /// <inheritdoc />
        protected override IPersonalAccountPaymentCommand GetCommand()
        {
            return new PersonalAccountAccumulatedFundPaymentCommand();
        }

        /// <inheritdoc />
        public override IDataResult Apply(IDistributionArgs args)
        {
            this.Validate(args);
            return this.ApplyInternal(args);
        }

        /// <inheritdoc />
        public override IEnumerable<BasePersonalAccount> GetPersonalAccounts(IQueryable<Transfer> query, ChargePeriod period)
        {
            return this.persaccDomain.GetAll()
                .Fetch(x => x.AccumulatedFundWallet)
                .Where(y => query.Any(x => x.Owner.Id == y.Id && x.TargetGuid == y.AccumulatedFundWallet.WalletGuid))
                .ToArray()
                .FetchWalletInTransfers(period, x => x.AccumulatedFundWallet);
        }

        /// <inheritdoc />
        public override IDistributionArgs ExtractArgsFrom(BaseParams baseParams)
        {
            return DistributionByAccountsArgs.FromParams(baseParams);
        }

        /// <inheritdoc />
        public override IDistributionArgs ExtractArgsFromMany(BaseParams baseParams, int counter, decimal thisOneDistribSum)
        {
            return DistributionByAccountsArgs.FromManyParams(baseParams, counter, thisOneDistribSum);
        }

        /// <summary>
        /// Утверждение
        /// </summary>
        /// <param name="args">Параметры распределения на ЛС</param>
        /// <exception cref="ArgumentException"></exception>
        protected virtual void Validate(IDistributionArgs args)
        {
            var distrArgs = args as DistributionByAccountsArgs;
            if (distrArgs == null)
            {
                throw new ArgumentException("args should be convertible to DistributionByAccountsArgs");
            }

            var distributable = distrArgs.Distributable;

            if (distributable == null)
            {
                throw new ArgumentException("Не удалось получить распределяемый объект");
            }

            var records = distrArgs.DistributionRecords;
            var distrsSumByAccountId = records.GroupBy(x => x.PersonalAccount.Id).ToDictionary(x => x.Key, x => x.Select(y => y.Sum).First());
            var accountIds = records.Select(x => x.PersonalAccount.Id).ToArray();

            var distrsByRo = this.persaccDomain.GetAll().Where(x => accountIds.Contains(x.Id))
                .Select(x => new
                {
                    roId = x.Room.RealityObject.Id,
                    x.Id
                })
                .ToList()
                .Select(x => new
                {
                    x.roId,
                    x.Id,
                    Sum = distrsSumByAccountId[x.Id]
                })
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, x => x);

            var realtyObjIds = distrsByRo.Select(x => x.Key).Distinct().ToList();

            var previousDistrSumsByRo = this.persaccDomain.GetAll()
                .Where(x => realtyObjIds.Contains(x.Room.RealityObject.Id))
                .Select(x => new
                {
                    roId = x.Room.RealityObject.Id,
                    x.PreviosWorkPaymentWallet.Balance,
                }).ToList().GroupBy(x => x.roId).ToDictionary(x => x.Key, x => x.Sum(y => y.Balance));

            foreach (var distr in distrsByRo)
            {
                var actualDecision = this.decisionService.GetActualDecision<AccumulationTransferDecision>(distr.Key);
                if (actualDecision == null)
                {
                    throw new ArgumentException("На доме нет решения о распределении ранее накопленных средств.");
                }

                if (actualDecision.Protocol.ProtocolDate > distributable.DateReceipt)
                {
                    throw new ArgumentException("Дата принятия решения в протоколе должна быть раньше, чем дата перевода ранее накопленных средств.");
                }

                var balance = actualDecision.Decision;
                if (previousDistrSumsByRo.ContainsKey(distr.Key))
                {
                    balance = balance - previousDistrSumsByRo[distr.Key];
                }

                if (balance < distr.Value.Sum(x => x.Sum))
                {
                    throw new ArgumentException("Сумма для перевода превышает сумму накопленных средств дома.");
                }
            }
        }
    }
}