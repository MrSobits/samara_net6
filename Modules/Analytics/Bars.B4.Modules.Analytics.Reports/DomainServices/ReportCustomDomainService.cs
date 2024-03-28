namespace Bars.B4.Modules.Analytics.Reports.DomainServices
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using System;

    public class ReportCustomDomainService : BaseDomainService<ReportCustom>
    {
        public override IDataResult Save(BaseParams baseParams)
        {
            var reports = new List<ReportCustom>();
            InTransaction(() =>
            {
                var result = base.Save(baseParams);
                reports = result.Data as List<ReportCustom>;
                if (reports != null && reports.Count == 1)
                {
                    SaveTemplate(baseParams, reports.First());
                }
            });
            return new BaseDataResult(reports.Select(x => new { x.Id, x.PrintFormat, x.CodedReportKey }));
        }

        public override IDataResult Update(BaseParams baseParams)
        {
            var reports = new List<ReportCustom>();
            InTransaction(() =>
            {
                var result = base.Update(baseParams);
                reports = result.Data as List<ReportCustom>;
                if (reports != null && reports.Count == 1)
                {
                    SaveTemplate(baseParams, reports.First());
                }
            });
            return new BaseDataResult(reports.Select(x => new { x.Id, x.PrintFormat, x.CodedReportKey }));
        }

        private void SaveTemplate(BaseParams baseParams, ReportCustom report)
        {
            if (baseParams.Files.Any())
            {
                var fileData = baseParams.Files["TemplateFile"];
                report.Template = fileData.Data;
            }
            else
            {
                report.Template = Convert.FromBase64String(baseParams.Params.GetAs<string>("Template"));
            }

            UpdateInternal(report);
        }
    }
}
