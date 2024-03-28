namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment.Payment
{
    using Domain.ValueObjects;
    using Entities;

    public class PersonalAccountPaymentByBaseTariffEvent : PersonalAccountPaymentEvent
    {
        public PersonalAccountPaymentByBaseTariffEvent(BasePersonalAccount account, MoneyStream money)
            : base(account, money)
        {
        }
    }
}