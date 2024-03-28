namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount
{
    using System;

    using Bars.Gkh.RegOperator.Enums;

    using Entities;
    using B4.Modules.FileStorage;

    using Bars.Gkh.DomainEvent.Infrastructure;

    public class PersonalAccountBalanceChangeEvent : IDomainEvent
    {
        public BasePersonalAccount Account { get; private set; }

        public decimal OldValue { get; private set; }

        public decimal NewValue { get; private set; }

        public WalletType WalletType { get; private set; }

        public DateTime OperationDate { get; private set; }

        public FileInfo BaseDoc { get; private set; }

        public string Reason { get; private set; }

        public PersonalAccountBalanceChangeEvent(
            BasePersonalAccount account,
            DateTime operationDate,
            WalletType walletType,
            decimal oldValue,
            decimal newValue,
            FileInfo baseDoc = null,
            string reason = null)
        {
            this.Account = account;
            this.OperationDate = operationDate;
            this.OldValue = oldValue;
            this.NewValue = newValue;
            this.BaseDoc = baseDoc;
            this.Reason = reason;

        }
    }
}