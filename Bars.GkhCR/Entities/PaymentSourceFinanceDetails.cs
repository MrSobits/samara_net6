namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Детализация по источнику финансирвоания оплаты акта выполненных работ
    /// </summary>
    public class PaymentSrcFinanceDetails : BaseImportableEntity
    {
        public PaymentSrcFinanceDetails()
        {
        }

        public PaymentSrcFinanceDetails(
            PerformedWorkActPayment actPayment,
            ActPaymentSrcFinanceType srcFinanceType,
            decimal balance, decimal payment)
        {
            ActPayment = actPayment;
            SrcFinanceType = srcFinanceType;
            Balance = balance;
            Payment = payment;
        }

        /// <summary>
        /// ссылка на оплату акта выполненных работ
        /// </summary>
        public virtual PerformedWorkActPayment ActPayment { get; set; }

        /// <summary>
        /// Тип источника финансирвоания
        /// </summary>
        public virtual ActPaymentSrcFinanceType SrcFinanceType { get; protected set; }

        /// <summary>
        /// Сальдо
        /// </summary>
        public virtual decimal Balance { get; protected set; }

        /// <summary>
        /// Оплата
        /// </summary>
        public virtual decimal Payment { get; protected set; }
    }
}