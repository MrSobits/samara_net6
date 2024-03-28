namespace Bars.Gkh.RegOperator.CodedReports
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Bars.B4.Application;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.Properties;

    public class DecisionContractReport : BaseCodedReport
    {
        protected override byte[] Template
        {
            get
            {
                return Resources.DecisionContractReport;
            }
        }

        public long DecisionProtocolId { get; set; }

        public override string Name
        {
            get { return "Договор протокола решений"; }
        }

        public override string Description { get; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            var provider = new DecisionContracDataProvider(ApplicationContext.Current.Container)
            {
                DecisionProtocolId = DecisionProtocolId
            };

            return new Collection<IDataSource>
            {
                new CodedDataSource("DecisionContract", provider)
            };
        }
    }
}