namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    using DomainModelServices;

    using Enums;

    using Newtonsoft.Json;

    using ValueObjects;

    using Wcf.Contracts.PersonalAccount;

    /// <summary>
    /// Пакет неподтвержденных оплат
    /// </summary>
    public class UnacceptedPaymentPacket : BaseImportableEntity, ITransferParty, IMoneyOperationSource
    {
        private IList<UnacceptedPayment> payments;

        /// <summary>
        /// NH
        /// </summary>
        public UnacceptedPaymentPacket()
        {
            this.TransferGuid = Guid.NewGuid().ToString();
            this.State = PaymentOrChargePacketState.Pending;
        }

        /// <summary>
        /// Дата создания
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// Описание пакета
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public virtual UnacceptedPaymentPacketType Type { get; set; }

        /// <summary>
        /// Общая сумма по пакету оплат
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        public virtual PaymentOrChargePacketState State { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual long? BankDocumentId { get; set; }

        [JsonIgnore]
        public virtual IEnumerable<UnacceptedPayment> Payments
        {
            get { return this.payments; }
        }

        [JsonIgnore]
        public virtual IEnumerable<UnacceptedPayment> UnacceptedPayments
        {
            get { return this.Payments.Where(x => !x.Accepted); }
        } 

        /// <summary>
        /// Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer
        /// </summary>
        public virtual string TransferGuid { get; protected set; }

        public virtual MoneyOperation CreateOperation(ChargePeriod period)
        {
            return new MoneyOperation(this.TransferGuid, period);
        }

        public virtual void Accept()
        {
            if (!EnumerableExtension.IsEmpty(this.Payments) && this.Payments.Any(x => !x.Accepted))
            {
                throw new InvalidOperationException("Some payments are not accepted");
            }

            this.State = PaymentOrChargePacketState.Accepted;
        }

        public virtual void Cancel()
        {
            if (!EnumerableExtension.IsEmpty(this.Payments) && this.Payments.Any(x => x.Accepted))
            {
                throw new InvalidOperationException("Some payments are accepted");
            }

            this.State = PaymentOrChargePacketState.Pending;
        }

        public virtual void SetInProgress()
        {
            this.State = PaymentOrChargePacketState.InProgress;
        }

        public virtual UnacceptedPayment CreatePayment(
            PersonalAccountPaymentInfoIn persAccPaymInfoIn, BasePersonalAccount personalAccount, UnacceptedPaymentTypePaid type)
        {
            var payment = new UnacceptedPayment
            {
                Packet = this,
                PersonalAccount = personalAccount,
                Guid = Guid.NewGuid().ToString(),
                Accepted = false,
                DocDate = persAccPaymInfoIn.DocumentDate,
                DocNumber = persAccPaymInfoIn.DocumentNumber,
                Comment = persAccPaymInfoIn.Details,
                PaymentDate = persAccPaymInfoIn.PaymentDate
            };

            switch (type)
            {
                case UnacceptedPaymentTypePaid.TargetPaid:
                    payment.Sum = persAccPaymInfoIn.TargetPaid;
                    payment.PenaltySum = 0m;
                    payment.PaymentType = PaymentType.Basic;
                    break;
                case UnacceptedPaymentTypePaid.SumPaid:
                    payment.Sum = persAccPaymInfoIn.SumPaid;
                    payment.PaymentType = PaymentType.Basic;
                    break;
                case UnacceptedPaymentTypePaid.PenaltyPaid:
                    payment.Sum = persAccPaymInfoIn.PenaltyPaid;
                    payment.PenaltySum = persAccPaymInfoIn.PenaltyPaid;
                    payment.PaymentType = PaymentType.Penalty;
                    break;
                case UnacceptedPaymentTypePaid.SocialSupport:
                    payment.Sum = persAccPaymInfoIn.SocialSupport;
                    payment.PenaltySum = 0m;
                    payment.PaymentType = PaymentType.SocialSupport;
                    break;
            }

            this.payments = this.payments ?? new List<UnacceptedPayment>();

            this.payments.Add(payment);

            return payment;
        }

        public enum UnacceptedPaymentTypePaid
        {
            TargetPaid = 1,
            SumPaid = 2,
            PenaltyPaid = 3,
            SocialSupport = 4
        }
    }
}