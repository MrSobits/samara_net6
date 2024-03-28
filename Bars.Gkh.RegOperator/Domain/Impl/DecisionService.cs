namespace Bars.Gkh.RegOperator.Domain.Impl
{
    using System.IO;
    using B4.IoC;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.Gkh.RegOperator.CodedReports;

    using Castle.Windsor;

    public class DecisionService : IDecisionService
    {
        private readonly IWindsorContainer _container;

        public DecisionService(IWindsorContainer container)
        {
            _container = container;
        }

        public Stream DownloadContract(BaseParams baseParams)
        {
            var decisionContractReport = new DecisionContractReport { DecisionProtocolId = baseParams.Params.GetAs<long>("decisionProtocolId") };

            var generator = _container.Resolve<ICodedReportManager>();
            using (_container.Using(generator))
            {
                return generator.GenerateReport(decisionContractReport, null, ReportPrintFormat.docx);
            }
        }
    }
}