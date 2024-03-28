namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment
{
    using Domain.ValueObjects;
    using Entities;
    using Payment;

    public class PersAccRestructAmicableAgreementPaymentEvent : PersonalAccountPaymentEvent
    {
        public PersAccRestructAmicableAgreementPaymentEvent(MoneyStream money, BasePersonalAccount account)
            : base(account, money)
        {
        }
    }
}