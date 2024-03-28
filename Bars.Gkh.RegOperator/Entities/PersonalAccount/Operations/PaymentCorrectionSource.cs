namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment.Payment;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Корректировка оплат
    /// </summary>
    public class PaymentCorrectionSource : PaymentOperationBase
    {
        private readonly IList<PaymentCorrection> paymentCorrections;
       
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="period">
        /// Период
        /// </param>
        public PaymentCorrectionSource(ChargePeriod period) : base(period)
        {
            this.PaymentSource = TypeTransferSource.PaymentCorrection;
            this.paymentCorrections = new List<PaymentCorrection>();
        }

        /// <summary>
        /// .ctor NHibernate
        /// </summary>
        protected PaymentCorrectionSource()
        {
            
        }
        
        /// <summary>
        /// Лицевой счёт
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Детализация корректировок
        /// </summary>
        public virtual IEnumerable<PaymentCorrection> PaymentCorrections => this.paymentCorrections;

        /// <summary>
        /// Применить корректировку оплат
        /// </summary>
        /// <param name="correctionInfo">Информация по корректировке</param>
        /// <returns>Трансферы</returns>
        public virtual PersonalAccountOperationResult ApplyCorrection(IList<PersonalAccountCorrectionInfo> correctionInfo)
        {
            if (correctionInfo.SafeSum(x => x.EnrollAmount) != correctionInfo.SafeSum(x => x.TakeAmount))
            {
                throw new ValidationException("Сумма снятия не равна сумме зачисления");
            }

            var summary = this.PersonalAccount.GetOpenedPeriodSummary();
            var correctionResult = new PersonalAccountOperationResult(this.CreateOperation(summary.Period));

            var baseTariffPayment = correctionInfo.Where(x => x.PaymentType == WalletType.BaseTariffWallet).Sum(x => x.Amount);
            var decisionTariffPayment = correctionInfo.Where(x => x.PaymentType == WalletType.DecisionTariffWallet).Sum(x => x.Amount);
            var penaltyPayment = correctionInfo.Where(x => x.PaymentType == WalletType.PenaltyWallet).Sum(x => x.Amount);

            foreach (var personalAccountCorrectionInfo in correctionInfo.Where(x => x.Amount < 0))
            {
                var wallet = this.GetWallet(personalAccountCorrectionInfo.PaymentType);

                var moneyStream = new MoneyStream(
                    this,
                    correctionResult.Operation,
                    this.OperationDate,
                    personalAccountCorrectionInfo.Amount)
                {
                    Description = $"Корректировка оплат {personalAccountCorrectionInfo.PaymentType.GetDisplayName().ToLower()}",
                    IsAffect = true,
                    OriginatorName = this.PersonalAccount.PersonalAccountNum
                };

                var transfer = wallet.StoreMoney(TransferBuilder.Create(this.PersonalAccount, moneyStream));
                correctionResult.AddTransfer(transfer);
                this.RaiseDomainEventIfNeed(personalAccountCorrectionInfo, moneyStream, transfer);
                this.ApplyCorrectionInternal(personalAccountCorrectionInfo);
            }

            foreach (var personalAccountCorrectionInfo in correctionInfo.Where(x => x.Amount > 0))
            {
                var wallet = this.GetWallet(personalAccountCorrectionInfo.PaymentType);

                var moneyStream = new MoneyStream(
                    this,
                    correctionResult.Operation,
                    this.OperationDate,
                    personalAccountCorrectionInfo.Amount)
                {
                    Description = $"Корректировка оплат {personalAccountCorrectionInfo.PaymentType.GetDisplayName().ToLower()}",
                    OriginatorName = this.PersonalAccount.PersonalAccountNum,
                    IsAffect = true
                };

                var transfer = wallet.StoreMoney(TransferBuilder.Create(this.PersonalAccount, moneyStream));
                correctionResult.AddTransfer(transfer);
                this.RaiseDomainEventIfNeed(personalAccountCorrectionInfo, moneyStream, transfer);
                this.ApplyCorrectionInternal(personalAccountCorrectionInfo);
            }

            // сальдо не обновляем, т.к. оно не изменилось
            summary.ApplyPayment(baseTariffPayment, decisionTariffPayment, penaltyPayment, false);

            return correctionResult;
        }

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <returns><see cref="MoneyOperation"/></returns>
        public override MoneyOperation CreateOperation(ChargePeriod period)
        {
            var operation = base.CreateOperation(period);
            operation.Reason = "Корректировка оплат";

            return operation;
        }

        private void ApplyCorrectionInternal(PersonalAccountCorrectionInfo info)
        {
            if (this.paymentCorrections.Any(x => x.PaymentType == info.PaymentType))
            {
                return;
            }

            this.paymentCorrections.Add(new PaymentCorrection(this, info.PaymentType)
            {
                EnrollAmount = info.EnrollAmount,
                TakeAmount = info.TakeAmount
            });
        }

        private Wallet GetWallet(WalletType walletType)
        {
            switch (walletType)
            {
                case WalletType.BaseTariffWallet:
                    return this.PersonalAccount.BaseTariffWallet;
                case WalletType.DecisionTariffWallet:
                    return this.PersonalAccount.DecisionTariffWallet;
                case WalletType.PenaltyWallet:
                    return this.PersonalAccount.PenaltyWallet;

                default:
                    throw new ArgumentOutOfRangeException(nameof(walletType), walletType, null);
            }
        }

        /// <summary>
        /// Вызов необходимых событий, чтобы создались трансферы на доме и события перерасчёта
        /// </summary>
        /// <param name="info">Данные по корректировке</param>
        /// <param name="source">Поток денег - источник</param>
        /// <param name="originalTransfer">Трансфер - источник</param>
        private void RaiseDomainEventIfNeed(PersonalAccountCorrectionInfo info, MoneyStream source, Transfer originalTransfer)
        {
            // зачисление средств по базовому
            if (info.PaymentType == WalletType.BaseTariffWallet && info.Amount != 0)
            {
                DomainEvents.Raise(new PersonalAccountPaymentByBaseTariffEvent(
                        this.PersonalAccount,
                        new MoneyStream(
                            this.PersonalAccount.BaseTariffWallet,
                            source.Operation,
                            source.OperationFactDate,
                            info.Amount)
                        {
                            OriginalTransfer = originalTransfer,
                            Description = originalTransfer.Reason,
                            OriginatorName = this.PersonalAccount.PersonalAccountNum,
                            IsAffect = true
                        }));
            }

            // зачисление средств по тарифу решения
            if (info.PaymentType == WalletType.DecisionTariffWallet && info.Amount != 0)
            {
                DomainEvents.Raise(new PersonalAccountPaymentByDecisionEvent(
                       new MoneyStream(
                           this.PersonalAccount.DecisionTariffWallet,
                           source.Operation,
                           source.OperationFactDate,
                           info.Amount)
                       {
                           OriginalTransfer = originalTransfer,
                           Description = originalTransfer.Reason,
                           OriginatorName = this.PersonalAccount.PersonalAccountNum,
                           IsAffect = true
                       },
                       this.PersonalAccount));
            }

            // зачисление средств по пени
            if (info.PaymentType == WalletType.PenaltyWallet && info.Amount != 0)
            {
                DomainEvents.Raise(new PersonalAccountPenaltyPaymentEvent(
                       new MoneyStream(
                           this.PersonalAccount.PenaltyWallet,
                           source.Operation,
                           source.OperationFactDate,
                           info.Amount)
                       {
                           OriginalTransfer = originalTransfer,
                           Description = originalTransfer.Reason,
                           OriginatorName = this.PersonalAccount.PersonalAccountNum,
                           IsAffect = true
                       },
                       this.PersonalAccount));
            }
        }
    }
}