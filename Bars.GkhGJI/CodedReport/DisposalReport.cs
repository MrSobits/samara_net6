namespace Bars.GkhGji.CodedReport
{
    using System.Collections.Generic;

    using Bars.B4.Application;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.GkhGji.Contracts.Meta;
    using Bars.GkhGji.DataProviders;
    using Bars.GkhGji.Properties;

    public class DisposalReport : BaseCodedReport
    {
        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new[]
            {
                new CodedDataSource(typeof(DisposalProxy).Name,
                    new DisposalGjiDataProvider(ApplicationContext.Current.Container))
            };
        }

        protected override byte[] Template
        {
            get { return Resources.Disposal; }
        }

        public override string Name
        {
            get { return "Приказ"; }
        }

        public override string Description { get; }
    }
}