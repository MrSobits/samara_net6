namespace Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount
{
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Обретка для создания трансферов начислений, если источник не является <see cref="IChargeOriginator"/>
    /// </summary>
    public class ChargeOriginatorWrapper : IChargeOriginator
    {
        private readonly ITransferParty source;
        private readonly MoneyOperation operation;

        /// <summary>
        /// Создать обретку для создания трансферов начислений, если источник не является <see cref="IChargeOriginator"/>
        /// </summary>
        /// <param name="source">Источник движения средств</param>
        /// <param name="operation">Операция, в рамках которой могут происходить различные движения денег</param>
        public ChargeOriginatorWrapper(ITransferParty source, MoneyOperation operation = null)
        {
            this.source = source;
            this.operation = operation;
        }

        /// <inheritdoc />
        public string TransferGuid => this.source.TransferGuid;

        /// <inheritdoc />
        public MoneyOperation CreateOperation(ChargePeriod period)
        {
            if (this.operation.IsNotNull())
            {
                return this.operation;
            }

            var moneyOperationSource = this.source as IMoneyOperationSource;

            if (moneyOperationSource.IsNotNull())
            {
                return moneyOperationSource.CreateOperation(period);
            }

            return new MoneyOperation(this.TransferGuid, period);
        }

        /// <inheritdoc />
        public TypeChargeSource ChargeSource => (this.source as IChargeOriginator)?.ChargeSource ?? 0;

        /// <inheritdoc />
        public string OriginatorGuid => this.TransferGuid;
    }
}