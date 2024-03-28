namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;

    /// <summary>
    ///  Детализация распоряжения об оплате акта выполненных работ
    /// </summary>
    public class PaymentOrderDetail : BaseImportableEntity
    {
        protected PaymentOrderDetail()
        {
        }

        public PaymentOrderDetail(PerformedWorkActPayment payment, Wallet.Wallet wallet, decimal amount)
        {
            PaymentOrder = payment;
            Wallet = wallet;
            Amount = amount;
            PaidSum = 0;
        }

        /// <summary>
        /// Распоряжение по оплате
        /// </summary>
        public virtual PerformedWorkActPayment PaymentOrder { get; protected set; }

        /// <summary>
        /// Кошелек, с которого надо вять деньги при оплате (Источник финансирования)
        /// </summary>
        public virtual Wallet.Wallet Wallet { get; protected set; }

        /// <summary>
        /// Сумма к оплате, руб. (сколько денег взять из кошелька)
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Оплаченная сумма
        /// </summary>
        public virtual decimal PaidSum { get; protected set; }

        /// <summary>
        /// Процедура оплаты
        /// </summary>
        /// <param name="amount"></param>
        public virtual void ApplyPayment(decimal amount)
        {
            if (Wallet.Balance < amount)
            {
                throw new Exception("На кошельке недостаточно денег.");
            }

            if (Amount < PaidSum + amount)
            {
                throw new Exception("Нельзя оплатить больше, чем того требуется.");
            }

            PaidSum += amount;
        }
    }
}
