namespace Bars.Gkh.RegOperator.DataProviders.PayDoc
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Castle.Windsor;

    internal class InvoiceRegistryAndActDataProvider : BaseInvoiceDataProvider<InvoiceInfo>
    {
        private readonly IEnumerable<PaymentDocumentSnapshot> snapshots;

        public InvoiceRegistryAndActDataProvider(IWindsorContainer container,
            IEnumerable<PaymentDocumentSnapshot> snapshots)
            : base(container, null)
        {
            this.snapshots = snapshots;
        }

        protected override IQueryable<InvoiceInfo> GetDataInternal(BaseParams baseParams)
        {
            var info = this.snapshots.Select(x => x.ConvertTo<InvoiceInfo>());

            return info.AsQueryable();
        }
    }
}