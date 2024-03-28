namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount
{
    using System;

    using Bars.Gkh.DomainEvent.Infrastructure;

    using Entities;

    public class PersonalAccountChangeDateOpenEvent : IDomainEvent
    {
        public BasePersonalAccount Account { get; private set; }

        public DateTime OldDateOpen { get; private set; }

        public DateTime NewDateOpen { get; private set; }

        public DateTime DateActual { get; private set; }

        public PersonalAccountChangeDateOpenEvent(
            BasePersonalAccount account,
            DateTime newDateOpen,
            DateTime oldDateOpen,
            DateTime dateActual)
        {
            Account = account;
            NewDateOpen = newDateOpen;
            OldDateOpen = oldDateOpen;
            DateActual = dateActual;
        }
    }
}
