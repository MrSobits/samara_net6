namespace Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner
{
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Хранитель владельца, помогает создавать трансферы без получения реальных сущностей
    /// </summary>
    public class TransferOwnerHolder : TransferOwner
    {
        private readonly long ownerId;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="ownerType">Тип владельца</param>
        /// <param name="ownerId">Идентификатор</param>
        public TransferOwnerHolder(WalletOwnerType ownerType, long ownerId)
        {
            this.TransferOwnerType = ownerType;
            this.ownerId = ownerId;
        }

        /// <inheritdoc />
        public override WalletOwnerType TransferOwnerType { get; }

        public Transfer TakeMoney(string walletGuid, TransferBuilder tranferBuilder)
        {
            var howMuch = tranferBuilder.MoneyStream;
            var amount = howMuch.Amount;
            if (amount == 0)
            {
                return null;
            }

            return tranferBuilder.SetSourceGuid(walletGuid).Build();
        }

        public Transfer StoreMoney(string walletGuid, TransferBuilder tranferBuilder)
        {
            var howMuch = tranferBuilder.MoneyStream;
            var amount = howMuch.Amount;
            if (amount == 0)
            {
                return null;
            }

            return tranferBuilder.SetTargetGuid(walletGuid).Build();
        }

        /// <inheritdoc />
        protected override Transfer CreateTransferInternal(string sourceGuid, string targetGuid, MoneyStream moneyStream)
        {
            if (this.TransferOwnerType == WalletOwnerType.BasePersonalAccount)
            {
                // если пришёл трансфер начисления
                if (moneyStream.Source is IChargeOriginator)
                {
                    return new PersonalAccountChargeTransfer(
                        new BasePersonalAccount { Id = this.ownerId },
                        sourceGuid,
                        targetGuid,
                        moneyStream.Amount,
                        moneyStream.Operation);
                }

                return new PersonalAccountPaymentTransfer(
                    new BasePersonalAccount { Id = this.ownerId },
                    sourceGuid,
                    targetGuid,
                    moneyStream.Amount,
                    moneyStream.Operation);
            }
            else
            {
                return new RealityObjectTransfer(
                    new RealityObjectPaymentAccount { Id = this.ownerId },
                    sourceGuid,
                    targetGuid,
                    moneyStream.Amount,
                    moneyStream.Operation);
            }
        }

        /// <inheritdoc />
        public override string GetDescription()
        {
            return this.TransferOwnerType.GetDisplayName();
        }
    }
}