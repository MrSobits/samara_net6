namespace Bars.Gkh.RegOperator.DataProviders.PayDoc
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Castle.Windsor;

    /// <summary>
    /// Поставщик данных для печати платежек
    /// </summary>
    internal class BaseInvoiceDataProvider<TModel> : BaseCollectionDataProvider<TModel>
        where TModel : InvoiceInfo, new()
    {
        private readonly IEnumerable<PaymentDocumentSnapshot> snapshots;

        public BaseInvoiceDataProvider(
            IWindsorContainer container,
            IEnumerable<PaymentDocumentSnapshot> snapshots)
            : base(container)
        {
            this.snapshots = snapshots;
        }

        public override string Name
        {
            get { return "Документ на оплату"; }
        }

        public override string Description
        {
            get { return this.Name; }
        }

        public override IEnumerable<DataProviderParam> Params
        {
            get { return new List<DataProviderParam>(); }
        }

        public override bool IsHidden
        {
            get { return true; }
        }

        protected override IQueryable<TModel> GetDataInternal(BaseParams baseParams)
        {
            var info = this.snapshots.Select(x => x.ConvertTo<TModel>());

            return info.AsQueryable();
        }
    }
}