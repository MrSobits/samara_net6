namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    public class PersonalAccountPenaltyChargeUndoEvent : IDomainEvent
    {
        public BasePersonalAccount Account { get; private set; }
        public decimal OldPenalty { get; private set; }
        public decimal NewPenalty { get; private set; }

        public MoneyOperation Operation { get; set; }
        public PersonalAccountChangeInfo ChangeInfo { get; set; }

        public PersonalAccountPenaltyChargeUndoEvent(BasePersonalAccount account, decimal oldPenalty, decimal newPenalty, PersonalAccountChangeInfo changeInfo)
        {
            Account = account;
            OldPenalty = oldPenalty;
            NewPenalty = newPenalty;
            ChangeInfo = changeInfo;
        }
    }
}