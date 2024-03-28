namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountPayment.UndoPayment
{
    using Bars.Gkh.Entities;

    using Entities;
    using Entities.ValueObjects;

    public class PersonalAccountRentUndoEvent : PersonalAccountPaymentUndoEvent
    {
        public PersonalAccountRentUndoEvent(BasePersonalAccount account, Transfer transfer, MoneyOperation operation, ChargePeriod period, string reason) : base(account, transfer, operation, period, reason)
        {
        }
    }
}