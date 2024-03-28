namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    using Loan;

    /// <summary>
    /// Связь между неподтвержденной оплатой и оплатой займа
    /// </summary>
    public class UnacceptedPaymentLoanPayment : BaseImportableEntity
    {
        /// <summary>
        /// Неподтвержденная оплата
        /// </summary>
        public virtual UnacceptedPayment UnacceptedPayment { get; set; }

        /// <summary>
        /// Оплата займа
        /// </summary>
        public virtual RealityObjectLoanPayment LoanPayment { get; set; }

        /// <summary>
        /// Сумма, которая пошла на оплату займа
        /// </summary>
        public virtual decimal PaymentSum { get; set; }
    }
}
