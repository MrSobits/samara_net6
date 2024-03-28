namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Entities;
    using Entities.PersonalAccount;
    using Enums;

    public interface IPersonalAccountHistoryCreator
    {
        PersonalAccountChange CreateChange(BasePersonalAccount account,
            PersonalAccountChangeType changeType,
            string description,
            string newValue,
            string oldValue,
            DateTime? actualFrom = null,
            FileInfo baseDoc = null,
            string reason = null);
    }
}