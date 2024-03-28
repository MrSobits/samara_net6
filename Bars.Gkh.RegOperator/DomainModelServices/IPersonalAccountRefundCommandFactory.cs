namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using Enums;

    /// <summary>
    /// Фабрика для получения реализаций IPersonalAccountRefundCommand
    /// </summary>
    public interface IPersonalAccountRefundCommandFactory
    {
        IPersonalAccountRefundCommand GetCommand(ImportedPaymentType paymentType);
    }
}
