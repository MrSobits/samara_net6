namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using Enums;

    /// <summary>
    /// Фабрика для получения реализаций IPersonalAccountPaymentCommand
    /// </summary>
    public interface IPersonalAccountPaymentCommandFactory
    {
        IPersonalAccountPaymentCommand GetCommand(PaymentType paymentType);

        IPersonalAccountPaymentCommand GetCommand(ImportedPaymentType paymentType);
    }
}