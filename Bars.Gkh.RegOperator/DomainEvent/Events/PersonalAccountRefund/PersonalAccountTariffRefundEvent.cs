namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountRefund
{
    using Domain.ValueObjects;
    using Entities;

    public class PersonalAccountTariffRefundEvent : PersonalAccountRefundEvent
    {
        public PersonalAccountTariffRefundEvent(BasePersonalAccount account, MoneyStream refund) : base(account, refund)
        {
        }
    }
}