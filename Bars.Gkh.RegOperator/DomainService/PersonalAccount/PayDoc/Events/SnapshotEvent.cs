namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.Events
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    internal class SnapshotEvent : IDomainEvent
    {
        public SnapshotEvent(PaymentDocumentSnapshot snapshot)
        {
            this.Snapshot = snapshot;
        }

        public PaymentDocumentSnapshot Snapshot { get; private set; }
    }
}