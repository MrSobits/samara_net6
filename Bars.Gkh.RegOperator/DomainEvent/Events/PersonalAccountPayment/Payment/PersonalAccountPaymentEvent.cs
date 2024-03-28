namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment.Payment
{
    using Bars.Gkh.DomainEvent.Infrastructure;

    using Domain.ValueObjects;
    using Entities;

    public abstract class PersonalAccountPaymentEvent : IDomainEvent
    {
        public BasePersonalAccount Account { get; protected set; }
        public MoneyStream Money { get; protected set; }

        protected PersonalAccountPaymentEvent(BasePersonalAccount account, MoneyStream money)
        {
            Account = account;
            Money = money;
        }
    }
}