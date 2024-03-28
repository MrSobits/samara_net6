namespace Bars.Gkh.RegOperator.Entities.Loan
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Оплата займа
    /// </summary>
    public class RealityObjectLoanPayment : BaseImportableEntity
    {
        /// <summary>
        /// Займ
        /// </summary>
        public virtual RealityObjectLoan Loan { get; set; }

        /// <summary>
        /// Операция прихода на счет
        /// </summary>
        public virtual RealityObjectPaymentAccountOperation IncomeOperation { get; set; }

        /// <summary>
        /// Операция оплаты со счета
        /// </summary>
        public virtual RealityObjectPaymentAccountOperation OutcomeOperation { get; set; }

        /// <summary>
        /// Гуид, связывающий источник операции с текущей оплатой займа.
        /// Пример источника - какое-либо распределение
        /// </summary>
        public virtual string OperationGuid { get; set; }
    }
}
