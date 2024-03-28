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
    public class PerformedWorkActPayment : BaseImportableEntity
    {
        public PerformedWorkActPayment()
        {
            TransferGuid = Guid.NewGuid().ToString();
        }

        private decimal _sum, _paid;

        public PerformedWorkActPayment(PerformedWorkAct act, ActPaymentType type, decimal sum, DateTime date) : this()
        {
            if (sum <= 0)
            {
                throw new ArgumentException(@"Сумма распоряжения должна быть положительным числом.", "sum");
            }
            PerformedWorkAct = act;
            DateDisposal = date;
            TypeActPayment = type;
            Sum = sum;
        }

        /// <summary>
        /// Акт выполненных работ, по которому было создано данное распоряжение
        /// </summary>
        public virtual PerformedWorkAct PerformedWorkAct { get; set; }

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
            get { return _sum; }
            set
            {
                _sum = value;
                Percent = Sum != 0 ? (Paid / Sum * 100).RoundDecimal(2) : 0M;
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
            get { return _paid; }
            set
            {
                _paid = value;
                Percent = Sum != 0 ? (Paid / Sum * 100).RoundDecimal(2) : 0M;
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
            if (Sum < Paid + payment)
            {
                throw new PerformedWorkActPaymentException(
                    "Нельзя оплатить больше суммы к оплате, указанной в предписании." +
                    string.Format("Сумма к оплате - {0}, оплачено - {1}, попытались оплатить - {2}", Sum, Paid, payment));
            }
            Paid += payment;
            DatePayment = paymentDate;
        }

        /// <summary>
        /// Отменить оплату по данному распоряжению
        /// </summary>
        /// <param name="amount"></param>
        public virtual void UndoPayment(decimal amount)
        {
            DatePayment = null;
            if (Paid < amount)
            {
                throw new PerformedWorkActPaymentException(
                    "Нельзя отменить оплату на сумму большую чем было оплачено"
                    + string.Format("Оплачено - {0}, сумма к отмене - {1}", Paid, amount));
            }

            Paid -= amount;
        }

        /// <summary>
        /// Применить изменения, пришедшие с клиента
        /// </summary>
        /// <param name="proxy"></param>
        public virtual void ApplyChanges(PerformedWorkActPaymentProxy proxy)
        {
            DateDisposal = proxy.DateDisposal;
            TypeActPayment = proxy.TypeActPayment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetActInfo()
        {
            return string.Format("№ {0} {1}",
                PerformedWorkAct.DocumentNum,
                PerformedWorkAct.DateFrom.HasValue ? "от {0}".FormatUsing(PerformedWorkAct.DateFrom.Value.ToShortDateString()): string.Empty);
        }
    }
}