namespace Bars.Gkh.RegOperator.DataProviders
{
    using System.Linq;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.Meta;

    using Castle.Windsor;

    /// <summary>
    /// Поставщик данных для искового заявления (сведения о собственниках)
    /// </summary>
    public class LawSuitOwnerForReportDataProvider : LawSuitOwnerDataProvider
    {
        public LawSuitOwnerForReportDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        protected override IQueryable<LawsuitProxy> GetDataInternal(BaseParams baseParams)
        {
            var records = new List<LawsuitProxy>() { new LawsuitProxy { Id = string.Join(", ", this.OwnerInfoIds) } };

            return records.AsQueryable();
        }
    }
}
