namespace Bars.GkhCr.Entities
{
    using System;
    using B4.Modules.FileStorage;
    using B4.Utils;

    using Bars.Gkh.Entities;

    using Enums;
    using Exceptions;

    /// <summary>
    /// Распоряжение к оплате акта.
    /// </summary>
    public class SpecialPerformedWorkActPayment : BaseImportableEntity
    {
        public SpecialPerformedWorkActPayment()
        {
            this.TransferGuid = Guid.NewGuid().ToString();
        }

        private decimal _sum, _paid;

        public SpecialPerformedWorkActPayment(SpecialPerformedWorkAct act, ActPaymentType type, decimal sum, DateTime date) : this()
        {
            if (sum <= 0)
            {
                throw new ArgumentException(@"Сумма распоряжения должна быть положительным числом.", "sum");
            }

            this.PerformedWorkAct = act;
            this.DateDisposal = date;
            this.TypeActPayment = type;
            this.Sum = sum;
        }

        /// <summary>
        /// Акт выполненных работ, по которому было создано данное распоряжение
        /// </summary>
        public virtual SpecialPerformedWorkAct PerformedWorkAct { get; set; }

        /// <summary>
        /// Дата распоряжения
        /// </summary>
        public virtual DateTime? DateDisposal { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime? DatePayment { get; set; }

        /// <summary>
        /// Вид оплаты
        /// </summary>
        public virtual ActPaymentType TypeActPayment { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual FileInfo Document { get; set; }

        /// <summary>
        /// Сумма к оплате, руб.
        /// </summary>
        public virtual decimal Sum
        {
            get { return this._sum; }
            set
            {
                this._sum = value;
                this.Percent = this.Sum != 0 ? (this.Paid / this.Sum * 100).RoundDecimal(2) : 0M;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual decimal Percent { get; protected set; }

        /// <summary>
        /// Не хранимое!
        /// Оплата в акте добавлена вручную
        /// костыль для 29236
        /// </summary>
        public virtual bool HandMade { get; set; }

        /// <summary>
        /// Сумма оплачено, руб
        /// </summary>
        public virtual decimal Paid
        {
            get { return this._paid; }
            set
            {
                this._paid = value;
                this.Percent = this.Sum != 0 ? (this.Paid / this.Sum * 100).RoundDecimal(2) : 0M;
            }
        }

        /// <summary>
        /// Guid трансфера денег для оплаты акта
        /// </summary>
        public virtual string TransferGuid { get; protected set; }

        /// <summary>
        /// Произвести оплату по данному распоряжению
        /// </summary>
        /// <param name="payment"></param>
        /// <param name="paymentDate"></param>
        public virtual void ApplyPayment(decimal payment, DateTime paymentDate)
        {
            if (this.Sum < this.Paid + payment)
            {
                throw new SpecialPerformedWorkActPaymentException(
                    "Нельзя оплатить больше суммы к оплате, указанной в предписании." +
                    $"Сумма к оплате - {this.Sum}, оплачено - {this.Paid}, попытались оплатить - {payment}");
            }

            this.Paid += payment;
            this.DatePayment = paymentDate;
        }

        /// <summary>
        /// Отменить оплату по данному распоряжению
        /// </summary>
        /// <param name="amount"></param>
        public virtual void UndoPayment(decimal amount)
        {
            this.DatePayment = null;
            if (this.Paid < amount)
            {
                throw new SpecialPerformedWorkActPaymentException(
                    "Нельзя отменить оплату на сумму большую чем было оплачено"
                    + $"Оплачено - {this.Paid}, сумма к отмене - {amount}");
            }

            this.Paid -= amount;
        }

        /// <summary>
        /// Применить изменения, пришедшие с клиента
        /// </summary>
        /// <param name="proxy"></param>
        public virtual void ApplyChanges(SpecialPerformedWorkActPaymentProxy proxy)
        {
            this.DateDisposal = proxy.DateDisposal;
            this.TypeActPayment = proxy.TypeActPayment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetActInfo()
        {
            return
                $"№ {this.PerformedWorkAct.DocumentNum} {(this.PerformedWorkAct.DateFrom.HasValue ? "от {0}".FormatUsing(this.PerformedWorkAct.DateFrom.Value.ToShortDateString()) : string.Empty)}";
        }
    }
}