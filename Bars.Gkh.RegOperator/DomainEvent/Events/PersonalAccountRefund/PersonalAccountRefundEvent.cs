namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountRefund
{
    using Bars.Gkh.DomainEvent.Infrastructure;

    using Domain.ValueObjects;
    using Entities;

    public abstract class PersonalAccountRefundEvent : IDomainEvent
    {
        public BasePersonalAccount Account { get; private set; }
        public MoneyStream Refund { get; private set; }

        protected PersonalAccountRefundEvent(BasePersonalAccount account, MoneyStream refund)
        {
            Account = account;
            Refund = refund;
        }
    }
}