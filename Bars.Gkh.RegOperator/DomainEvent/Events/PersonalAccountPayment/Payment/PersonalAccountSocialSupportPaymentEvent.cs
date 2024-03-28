namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment.Payment
{
    using Domain.ValueObjects;
    using Entities;

    public class PersonalAccountSocialSupportPaymentEvent : PersonalAccountPaymentEvent
    {
        public PersonalAccountSocialSupportPaymentEvent(MoneyStream money, BasePersonalAccount account)
            : base(account, money)
        {
        }
    }
}