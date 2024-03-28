namespace Bars.Gkh.RegOperator.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Castle.Windsor;

    internal class InvoiceAccountInfoDataProvider : BaseCollectionDataProvider<AccountInfo>
    {
        private readonly IEnumerable<AccountPaymentInfoSnapshot> snapshots;

        public InvoiceAccountInfoDataProvider(IWindsorContainer container,
            IEnumerable<AccountPaymentInfoSnapshot> snapshots)
            : base(container)
        {
            this.snapshots = snapshots;
        }

        public override string Name
        {
            get { return "Данные по счетам"; }
        }

        public override string Description
        {
            get { return Name; }
        }

        public override IEnumerable<DataProviderParam> Params
        {
            get { return new List<DataProviderParam>(); }
        }

        protected override IQueryable<AccountInfo> GetDataInternal(BaseParams baseParams)
        {
            var info = this.snapshots.Select(x => x.ConvertTo<AccountInfo>());

            return info.AsQueryable();
        }
    }
}