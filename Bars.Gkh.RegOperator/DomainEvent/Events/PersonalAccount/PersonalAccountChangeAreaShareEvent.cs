namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount
{
    using System;
    using Entities;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.DomainEvent.Infrastructure;

    public class PersonalAccountChangeAreaShareEvent : IDomainEvent
    {
        public BasePersonalAccount Account { get; private set; }

        public decimal OldAreaShare { get; private set; }

        public decimal NewAreaShare { get; private set; }

        public DateTime DateActual { get; private set; }

        public FileInfo Document { get; private set; }

        public PersonalAccountChangeAreaShareEvent(
            BasePersonalAccount account,
            decimal newAreaShare,
            decimal oldAreaShare,
            DateTime dateActual,
            FileInfo document)
        {
            Account = account;
            NewAreaShare = newAreaShare;
            OldAreaShare = oldAreaShare;
            DateActual = dateActual;
            Document = document;
        }
    }
}