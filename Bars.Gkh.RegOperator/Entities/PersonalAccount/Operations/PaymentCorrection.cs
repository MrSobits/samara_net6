namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Корректировка оплат по типу кошелька
    /// </summary>
    public class PaymentCorrection : BaseImportableEntity
    {
        /// <summary>
        /// .ctor NH
        /// </summary>
        public PaymentCorrection()
        {
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="source">Источник движения</param>
        /// <param name="paymentType">Тип движения</param>
        public PaymentCorrection(PaymentCorrectionSource source, WalletType paymentType)
        {
            this.PaymentType = paymentType;
            this.PaymentOperation = source;
        }

        /// <summary>
        /// Операция оплаты
        /// </summary>
        public virtual PaymentOperationBase PaymentOperation { get; set; }

        /// <summary>
        /// Тип оплат
        /// </summary>
        public virtual WalletType PaymentType { get; set; }

        /// <summary>
        /// Снять
        /// </summary>
        public virtual decimal TakeAmount { get; set; }

        /// <summary>
        /// Зачислить
        /// </summary>
        public virtual decimal EnrollAmount { get; set; }
    }
}