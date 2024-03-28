namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment
{
    using Domain.ValueObjects;
    using Entities;
    using Payment;

    public class PersonalAccountAccumulatedFundPaymentEvent : PersonalAccountPaymentEvent
    {
        public PersonalAccountAccumulatedFundPaymentEvent(MoneyStream money, BasePersonalAccount account)
            : base(account, money)
        {
        }
    }
}