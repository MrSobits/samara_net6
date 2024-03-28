namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountRefund
{
    using Domain.ValueObjects;
    using Entities;

    public class PersonalAccountDecisionRefundEvent : PersonalAccountRefundEvent
    {
        public PersonalAccountDecisionRefundEvent(BasePersonalAccount account, MoneyStream refund) : base(account, refund)
        {
        }
    }
}