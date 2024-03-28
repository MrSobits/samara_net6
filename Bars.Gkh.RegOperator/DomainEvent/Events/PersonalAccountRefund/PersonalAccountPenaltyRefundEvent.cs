namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountRefund
{
    using Domain.ValueObjects;
    using Entities;

    public class PersonalAccountPenaltyRefundEvent : PersonalAccountRefundEvent
    {
        public PersonalAccountPenaltyRefundEvent(BasePersonalAccount account, MoneyStream refund) : base(account, refund)
        {
        }
    }
}