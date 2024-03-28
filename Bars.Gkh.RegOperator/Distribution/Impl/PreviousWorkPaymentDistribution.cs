namespace Bars.Gkh.RegOperator.Distribution
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
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
    /// Средства за ранее выполненные работы
    /// </summary>
    public class PreviousWorkPaymentDistribution : AbstractPersonalAccountDistribution
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
        public PreviousWorkPaymentDistribution(
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
        public override DistributionCode DistributionCode => DistributionCode.PreviousWorkPaymentDistribution;

        /// <inheritdoc />
        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.PreviousWorkPayments";

        /// <inheritdoc />
        public override IEnumerable<BasePersonalAccount> GetPersonalAccounts(IQueryable<Transfer> query, ChargePeriod period)
        {
            return this.persaccDomain.GetAll()
                .Fetch(x => x.PreviosWorkPaymentWallet)
                .Where(y => query.Any(x => x.Owner.Id == y.Id))
                .ToArray()
                .FetchWalletInTransfers(period, x => x.PreviosWorkPaymentWallet);
        }

        /// <inheritdoc />
        protected override IPersonalAccountPaymentCommand GetCommand()
        {
            return new PersonalAccountPreviosWorkPaymentCommand();
        }

        /// <inheritdoc />
        public override IDataResult Apply(IDistributionArgs args)
        {
            return this.ApplyInternal(args);
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
    }
}