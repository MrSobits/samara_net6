namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.Events
{
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    internal class AccountSnapshotEvent : IDomainEvent
    {
        public AccountSnapshotEvent(AccountPaymentInfoSnapshot snapshot, long snapshotParentId, string snapshotType, bool isBase = true)
        {
            this.Snapshot = snapshot;
            this.SnapshotParentId = snapshotParentId;
            this.SnapshotType = snapshotType;
            this.IsBase = isBase;
        }

        public AccountPaymentInfoSnapshot Snapshot { get; private set; }
        public long SnapshotParentId { get; private set; }
        public string SnapshotType { get; private set; }
        public bool IsBase { get; private set; }
    }
}