namespace Bars.Gkh.DomainService.Impl
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;

    using Castle.Windsor;

    public class TemplateReplacementService : ITemplateReplacementService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListReports(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var listReports = Container.ResolveAll<IGkhBaseReport>()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Description
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(listReports.Order(loadParams).Paging(loadParams).ToList(), listReports.Count());
        }

        public IDataResult GetBaseTemplate(BaseParams baseParams)
        {
            var reportId = baseParams.Params["reportId"].ToStr();
            var id = baseParams.Params["id"].ToStr();

            var report = Container.ResolveAll<IGkhBaseReport>().FirstOrDefault(x => x.Id == reportId);

            if (report != null)
            {
                var template = report.GetTemplateInfo().FirstOrDefault(x => x.Code == id);

                if (template != null)
                {

                    var extention = report.Extention;

                    if (string.IsNullOrEmpty(extention))
                    {
                        // Если программист забыл заполнить поле Extention при указани шаблона то остается только по генератору определять что он хотел
                        switch (report.ReportGeneratorName)
                        {
                            case "XlsIoGenerator": extention = "xls"; break;
                            case "DocIoGenerator": extention = "doc"; break;
                            case "StimulReportGenerator": extention = "mrt"; break;
                        } 
                    }

                    return new BaseDataResult(template) { Success = true, Message = extention};
                }
            }

            return new BaseDataResult();
        }
    }
}