namespace Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount
{
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Инициатор оплаты
    /// </summary>
    public interface IPaymentOriginator : ITransferParty, IMoneyOperationSource
    {
        /// <summary>
        /// Тип источника поступления
        /// </summary>
        TypeTransferSource PaymentSource { get; }

        /// <summary>
        /// Guid инициатора
        /// </summary>
        string OriginatorGuid { get; }
    }
}