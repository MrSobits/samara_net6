namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment
{
    using System;

    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Событие на закрытие ЛС
    /// </summary>
    public class PersonalAccountCloseEvent : IDomainEvent
    {
        public PersonalAccountCloseEvent(BasePersonalAccount account, DateTime closeDate)
        {
            this.Account = account;
            this.CloseDate = closeDate;
        }

        public BasePersonalAccount Account { get; private set; }

        public DateTime CloseDate { get; private set; }
    }
}