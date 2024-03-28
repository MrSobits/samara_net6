namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;

    public class TemplateReplacementViewModel : BaseViewModel<TemplateReplacement>
    {
        public override IDataResult List(IDomainService<TemplateReplacement> domain, BaseParams baseParams)
        {
            var reportId = baseParams.Params.GetAs<string>("reportId");

            var report = Container.ResolveAll<IGkhBaseReport>().FirstOrDefault(x => x.Id == reportId);

            if (report != null)
            {
                var templs = domain
                    .GetAll()
                    .Select(x => x.Code)
                    .ToArray();

                var loadParams = baseParams.GetLoadParam();

                var extension = report.Extention.IsEmpty() ? 
                        report.ReportGeneratorName == "XlsIoGenerator" ? "xls" : 
                        report.ReportGeneratorName == "DocIoGenerator" ? "doc" : 
                        report.ReportGeneratorName == "StimulReportGenerator" ? "mrt" : ""
                    : report.Extention;

                var templates =
                    report.GetTemplateInfo()
                          .Select(x => new
                          {
                              x.Name, 
                              x.Code, 
                              x.Description, 
                              HasReplace = templs.Contains(x.Code),
                              Extension = extension
                          })
                          .AsQueryable().Filter(loadParams, Container);

                var totalCount = templates.Count();

                templates = templates.Order(loadParams).Paging(loadParams);

                return new ListDataResult(templates.ToList(), totalCount);
            }

            return new ListDataResult(null, 0);
        }

        public override IDataResult Get(IDomainService<TemplateReplacement> domain, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs("id", string.Empty);

            var replace = domain.GetAll().FirstOrDefault(x => x.Code.Equals(id));

            return replace != null ?
                new BaseDataResult(replace) :
                new BaseDataResult(new { Code = id, Template = (FileInfo)null });
        }
    }
}
