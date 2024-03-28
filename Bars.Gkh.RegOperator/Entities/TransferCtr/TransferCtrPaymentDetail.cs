namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Детализация оплат заявки на перечисление средств подрядчикам
    /// </summary>
    public class TransferCtrPaymentDetail : BaseImportableEntity
    {
        public TransferCtrPaymentDetail()
        {
            
        }

        public TransferCtrPaymentDetail(TransferCtr transferCtr, Wallet.Wallet wallet)
        {
            TransferCtr = transferCtr;
            Wallet = wallet;
        }

        /// <summary>
        /// Заявка на перечисление средств подрядчикам
        /// </summary>
        public virtual TransferCtr TransferCtr { get; set; }

        /// <summary>
        /// Кошелек
        /// </summary>
        public virtual Wallet.Wallet Wallet { get; set; }

        /// <summary>
        /// Сумма к оплате
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Оплаченная сумма
        /// </summary>
        public virtual decimal PaidSum { get; set; }

        /// <summary>
        /// Сумма возврата
        /// </summary>
        public virtual decimal RefundSum { get; set; }
    }
}