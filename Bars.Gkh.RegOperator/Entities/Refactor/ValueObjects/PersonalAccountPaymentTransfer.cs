namespace Bars.Gkh.RegOperator.Entities.ValueObjects
{

    /// <summary>
    /// Трансфер оплаты на ЛС
    /// </summary>
    public class PersonalAccountPaymentTransfer : TransferWithOwner<BasePersonalAccount>
    {
        /// <inheritdoc />
        public PersonalAccountPaymentTransfer(BasePersonalAccount owner, string sourceGuid, string targetGuid, decimal amount, MoneyOperation operation)
            : base(owner, sourceGuid, targetGuid, amount, operation)
        {
        }

        /// <summary>
        /// .ctor NH
        /// </summary>
        protected PersonalAccountPaymentTransfer()
        {
        }

        /// <summary>
        /// Первоначальный перевод. В случае отката трансфера здесь будет ссылка на первоначальный перевод
        /// </summary>
        public new virtual PersonalAccountPaymentTransfer Originator
        {
            get { return (PersonalAccountPaymentTransfer)base.Originator; }
            set { base.Originator = value; }
        }
    }
}