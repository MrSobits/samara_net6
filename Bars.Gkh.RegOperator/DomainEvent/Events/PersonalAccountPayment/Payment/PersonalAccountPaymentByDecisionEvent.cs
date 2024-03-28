namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment.Payment
{
    using Domain.ValueObjects;
    using Entities;

    public class PersonalAccountPaymentByDecisionEvent : PersonalAccountPaymentEvent
    {
        public PersonalAccountPaymentByDecisionEvent(MoneyStream money, BasePersonalAccount account)
            : base(account, money)
        {
        }
    }
}