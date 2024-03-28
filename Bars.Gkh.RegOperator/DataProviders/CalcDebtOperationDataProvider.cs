namespace Bars.Gkh.RegOperator.DataProviders
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.Meta;

    using Castle.Windsor;
    using System.Collections.Generic;

    public class CalcDebtOperationDataProvider : BaseCollectionDataProvider<CalcDebtOperationProxy>
    {
        public CalcDebtOperationDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        public override string Name
        {
            get { return "Отчёт по переносу долга в ЧЭС"; }
        }

        public override string Description
        {
            get { return this.Name; }
        }

        public string CalcDebtId { get; set; }

        protected override IQueryable<CalcDebtOperationProxy> GetDataInternal(BaseParams baseParams)
        {
            var record = new CalcDebtOperationProxy { Id = this.CalcDebtId };

            var records = new List<CalcDebtOperationProxy> { record };

            return records.AsQueryable();
        }
    }
}