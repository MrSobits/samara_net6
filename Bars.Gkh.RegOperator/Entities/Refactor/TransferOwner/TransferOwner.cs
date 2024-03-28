namespace Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Абстрактный владелец трансфера
    /// </summary>
    public abstract class TransferOwner : BaseImportableEntity, ITransferOwner
    {
        /// <inheritdoc />
        public abstract WalletOwnerType TransferOwnerType { get; }

        /// <inheritdoc />
        public virtual Transfer CreateTransfer(string sourceGuid, string targetGuid, MoneyStream moneyStream)
        {
            var transfer = this.CreateTransferInternal(sourceGuid, targetGuid, moneyStream);

            transfer.PaymentDate = moneyStream.OperationFactDate;
            transfer.OperationDate = moneyStream.OperationDate;
            transfer.Reason = moneyStream.Description;
            transfer.Originator = moneyStream.OriginalTransfer;
            transfer.IsAffect = moneyStream.IsAffect;
            transfer.OriginatorName = moneyStream.OriginatorName;

            return transfer;
        }

        /// <summary>
        /// Метод создания трансфера
        /// </summary>
        /// <param name="sourceGuid">Источник</param>
        /// <param name="targetGuid">Целевой гуид</param>
        /// <param name="moneyStream">Поток средств</param>
        /// <returns>Трансфер</returns>
        protected abstract Transfer CreateTransferInternal(string sourceGuid, string targetGuid, MoneyStream moneyStream);

        /// <inheritdoc />
        public abstract string GetDescription();
    }
}