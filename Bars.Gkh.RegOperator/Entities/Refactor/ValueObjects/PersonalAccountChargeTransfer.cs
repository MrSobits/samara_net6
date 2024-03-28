namespace Bars.Gkh.RegOperator.Entities.ValueObjects
{

    /// <summary>
    /// Трансфер начисления ЛС
    /// </summary>
    public class PersonalAccountChargeTransfer : TransferWithOwner<BasePersonalAccount>
    {
        /// <inheritdoc />
        public PersonalAccountChargeTransfer(BasePersonalAccount owner, string sourceGuid, string targetGuid, decimal amount, MoneyOperation operation)
            : base(owner, sourceGuid, targetGuid, amount, operation)
        {
        }

        /// <summary>
        /// .ctor NH
        /// </summary>
        protected PersonalAccountChargeTransfer()
        {
        }

        /// <summary>
        /// Первоначальный перевод. В случае отката трансфера здесь будет ссылка на первоначальный перевод
        /// </summary>
        public new virtual PersonalAccountChargeTransfer Originator
        {
            get { return (PersonalAccountChargeTransfer)base.Originator; }
            set { base.Originator = value; }
        }
    }
}