namespace Bars.Gkh.RegOperator.Entities.ValueObjects
{
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;

    /// <summary>
    /// Нехранимый трансфер
    /// </summary>
    public class FakeTransfer : Transfer
    {
        /// <inheritdoc cref="Transfer"/>
        public FakeTransfer(ITransferOwner owner, string sourceGuid, string targetGuid, decimal sum, MoneyOperation moneyOperation)
            : base(owner, sourceGuid, targetGuid, sum, moneyOperation)
        {
            
        }
    }
}