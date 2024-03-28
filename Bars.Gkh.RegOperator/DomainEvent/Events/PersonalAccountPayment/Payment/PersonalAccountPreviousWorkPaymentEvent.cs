namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment.Payment
{
    using Domain.ValueObjects;
    using Entities;

    public class PersonalAccountPreviousWorkPaymentEvent : PersonalAccountPaymentEvent
    {
        public PersonalAccountPreviousWorkPaymentEvent(MoneyStream money, BasePersonalAccount account)
            : base(account, money)
        {
        }
    }
}