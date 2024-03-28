namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment
{
    using Bars.Gkh.DomainEvent.Infrastructure;

    using Entities;

    /// <summary>
    /// Событие погашение долга по ЛС
    /// </summary>
    public class PersonalAccountDebtIsZeroEvent : IDomainEvent
    {
        public BasePersonalAccount Account { get; private set; }

        public PersonalAccountDebtIsZeroEvent(BasePersonalAccount account)
        {
            Account = account;
        }
    }
}