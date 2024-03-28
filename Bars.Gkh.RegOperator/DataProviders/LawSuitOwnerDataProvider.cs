namespace Bars.Gkh.RegOperator.DataProviders
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.Meta;

    using Castle.Windsor;

    /// <summary>
    /// Поставщик данных для искового заявления (сведения о собственниках)
    /// </summary>
    public class LawSuitOwnerDataProvider : BaseCollectionDataProvider<LawsuitProxy>
    {
        public LawSuitOwnerDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        public override string Name
        {
            get { return "Исковое заявление (сведения о собственниках)"; }
        }

        public override string Description
        {
            get { return this.Name; }
        }

        public string LawSuitId { get; set; }

        public long[] OwnerInfoIds { get; set; }

        protected override IQueryable<LawsuitProxy> GetDataInternal(BaseParams baseParams)
        {
            var records = this.OwnerInfoIds.Select(x => new LawsuitProxy
            {
                Id = x.ToString()
            });

            return records.AsQueryable();

          
        }
    }
}