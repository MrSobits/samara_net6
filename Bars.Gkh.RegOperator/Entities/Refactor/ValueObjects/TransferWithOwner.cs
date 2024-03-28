namespace Bars.Gkh.RegOperator.Entities.ValueObjects
{
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;

    /// <summary>
    /// Трансфер с владельцем
    /// </summary>
    /// <typeparam name="TOwner">Владелец трансфера</typeparam>
    public abstract class TransferWithOwner<TOwner> : Transfer, ITransfer<TOwner> where TOwner : class, ITransferOwner
    {
        /// <inheritdoc />
        public new virtual TOwner Owner
        {
            get { return (TOwner)base.Owner; }
            set { base.Owner = value; }
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="owner">Владелец трансфера</param>
        /// <param name="sourceGuid">Гуид источник</param>
        /// <param name="targetGuid">Целевой гуид</param>
        /// <param name="amount">Сумма</param>
        /// <param name="operation">Операция</param>
        protected TransferWithOwner(TOwner owner, string sourceGuid, string targetGuid, decimal amount, MoneyOperation operation)
            : base(owner, sourceGuid, targetGuid, amount, operation)
        {
        }

        /// <summary>
        /// .ctor NH
        /// </summary>
        protected TransferWithOwner()
        {
        }
    }
}