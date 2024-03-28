namespace Bars.Gkh.Decisions.Nso.Domain.Decision.Impl
{
    using System;
    using System.IO;
    using B4;
    using B4.Modules.Reports;
    using Castle.Windsor;
    using Decisions;
    using Gkh.Report;

    public class DecisionNotificationService : IDecisionNotificationService
    {
        protected IWindsorContainer Container;

        public DecisionNotificationService(IWindsorContainer container)
        {
            Container = container;
        }

        public Stream DownloadNotification(BaseParams baseParams)
        {
            var generator = Container.Resolve<IReportGenerator>("XlsIoGenerator");

            var number = baseParams.Params.GetAs<string>("Number");
            var mu = baseParams.Params.GetAs<string>("Mu");
            var moSettlement = baseParams.Params.GetAs<string>("MoSettlement");
            var address = baseParams.Params.GetAs<string>("Address");
            var manage = baseParams.Params.GetAs<string>("Manage");
            var formFundType = baseParams.Params.GetAs<string>("FormFundType");
            var orgName = baseParams.Params.GetAs<string>("OrgName");
            
            var reportParams = new ReportParams();
            reportParams.SimpleReportParams["Number"] = number;
            reportParams.SimpleReportParams["Date"] = DateTime.Now.ToString("dd.MM.yyyy");

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            section.ДобавитьСтроку();
            section["Mu"] = mu;
            section["MoSettlement"] = moSettlement;
            section["Address"] = address;
            section["Manage"] = manage;
            section["FormFundType"] = formFundType;
            section["OrgName"] = orgName;

            var template = new MemoryStream(Properties.Resources.DecisionNotificationForm);

            var result = new MemoryStream();

            generator.Open(template);
            generator.Generate(result, reportParams);
            result.Seek(0, SeekOrigin.Begin);
    
            Container.Release(generator);

            return result;
        }
    }
}