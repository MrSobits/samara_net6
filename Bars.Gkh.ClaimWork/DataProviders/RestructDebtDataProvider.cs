namespace Bars.Gkh.ClaimWork.DataProviders
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.Analytics.Data;

    using Castle.Windsor;
    using Meta;

    /// <summary>
    /// Поставщик данных для печати соглашения о погашении задолженности
    /// </summary>
    public class RestructDebtDataProvider : BaseCollectionDataProvider<DocumentClwProxy>
    {
        public RestructDebtDataProvider(IWindsorContainer container, string name)
            : base(container)
        {
            this.Name = name;
        }

        public override string Name { get; }

        public override string Description => this.Name;

        public string DebtorClaimWorkId { get; set; }

        protected override IQueryable<DocumentClwProxy> GetDataInternal(BaseParams baseParams)
        {
            var record = new DocumentClwProxy { Id = this.DebtorClaimWorkId };

            var records = new List<DocumentClwProxy> { record };

            return records.AsQueryable();
        }
    }
}