namespace Bars.Gkh.RegOperator.Distribution.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
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
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;

    using NHibernate.Linq;

    /// <summary>
    /// Возврат МСП
    /// </summary>
    public class RefundMspDistribution : AbstractPersonalAccountDistribution
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
        public RefundMspDistribution(
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
        public override DistributionCode DistributionCode => DistributionCode.RefundMspDistribution;

        public override string PermissionId => "GkhRegOp.Accounts.SuspenseAccount.EnrollmentTypes.RefundMsp";

        public override bool CanApply(IDistributable distributable)
        {
            return distributable.MoneyDirection == MoneyDirection.Outcome;
        }

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

        protected override IPersonalAccountPaymentCommand GetCommand()
        {
            throw new NotImplementedException();
        }

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

            var command = new PersonalAccountRefundCommand(RefundType.Msp);

            var details = new List<DistributionDetail>();

            var transfers = new List<Transfer>();
            foreach (var record in records)
            {
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

        public override void Undo(IDistributable distributable, MoneyOperation operation)
        {
            var transferQuery = this.transferRepo.GetByMoneyOperation(operation);

            var period = this.chargePeriodRepo.GetCurrentPeriod();
            var cancelOperation = distributable.CancelOperation(operation, period);

            var command = new PersonalAccountRefundCommand(RefundType.Msp);

            var accounts = this.GetPersonalAccounts(transferQuery, operation.Period);
            var transfers = new List<Transfer>();

            foreach (var account in accounts)
            {
                transfers.AddRange(account.UndoRefund(command, cancelOperation, period, distributable.DateReceipt));
            }

            transfers.ForEach(this.transferDomain.Save);
        }
    }
}