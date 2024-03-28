using System.IO;
using Bars.B4.Modules.DataExport;

namespace Bars.Gkh.Modules.ClaimWork.Export
{
    using B4.Modules.Reports;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Castle.Windsor;
    using System.Collections;
    using System.Collections.Generic;

    using Bars.B4;

    public class BaseClaimWorkExport<T> : IBaseClaimWorkExport<T>
        where T : BaseClaimWork
    {
        public IWindsorContainer Container { get; set; }

        public virtual IList GetExportData(BaseParams baseParams)
        {

            var service = Container.Resolve<IBaseClaimWorkService<T>>();

            try
            {
                var totalCount = 0;
                return service.GetList(baseParams, false, ref totalCount);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public virtual ReportStreamResult ExportData(BaseParams baseParams)
        {
            var generator = Container.Resolve<IReportGenerator>("XlsIoGenerator");

            var report = this.Container.Resolve<IDataExportReport>("DataExportReport.PrintForm",
                new[]
                {
                    new KeyValuePair<string, object>("Data", GetExportData(baseParams)),
                    new KeyValuePair<string, object>("BaseParams", baseParams),
                });

            var rp = new ReportParams();

            report.PrepareReport(rp);
            var template = report.GetTemplate();

            var result = new MemoryStream();

            generator.Open(template);
            generator.Generate(result, rp);
            result.Seek(0, SeekOrigin.Begin);

            Container.Release(report);
            Container.Release(generator);

            return new ReportStreamResult(result, "export.xlsx");
        }
    }
}