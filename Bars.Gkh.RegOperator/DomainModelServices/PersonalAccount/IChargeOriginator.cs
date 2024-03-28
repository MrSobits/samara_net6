namespace Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount
{
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Инициатор изменения начислений
    /// </summary>
    public interface IChargeOriginator : ITransferParty, IMoneyOperationSource
    {
        /// <summary>
        /// Тип источника поступления начислений
        /// </summary>
        TypeChargeSource ChargeSource { get; }

        /// <summary>
        /// Guid инициатора
        /// </summary>
        string OriginatorGuid { get; }
    }
}
