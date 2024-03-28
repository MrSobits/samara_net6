namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount
{
    using System;

    using Bars.Gkh.DomainEvent.Infrastructure;

    using Entities;

    public class PersonalAccountChangeOwnerEvent : IDomainEvent
    {
        public BasePersonalAccount Account { get; private set; }

        public PersonalAccountOwner NewOwner { get; private set; }

        public PersonalAccountOwner OldOwner { get; private set; }

        public PersonalAccountChangeInfo ChangeInfo { get; private set; }

        public PersonalAccountChangeOwnerEvent(
            BasePersonalAccount account,
            PersonalAccountOwner newOwner,
            PersonalAccountOwner oldOwner,
            PersonalAccountChangeInfo changeInfo)
        {
            Account = account;
            NewOwner = newOwner;
            OldOwner = oldOwner;
            ChangeInfo = changeInfo;
        }
    }
}