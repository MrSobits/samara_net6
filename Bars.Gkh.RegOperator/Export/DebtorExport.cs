namespace Bars.Gkh.RegOperator.Export
{
    using B4;
    using B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Entity;
    using DomainService.PersonalAccount;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Text;

    using Castle.MicroKernel;

    using IReportGenerator = B4.Modules.Reports.IReportGenerator;

    public class DebtorExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var service = Container.Resolve<IDebtorService>();

            try
            {
                var data = service.GetListQuery(baseParams, out int totalCount);
                // as List<ViewDebtorExport>;
                var bigData = new List<List<ViewDebtorExport>>();

                if (totalCount > 50000)
                {
                    int i = 0;
                    while (totalCount > 50000)
                    {
                        List<ViewDebtorExport> smalList = data.Skip(i * 50000).Take(50000).ToList<ViewDebtorExport>();
                        i++;
                        bigData.Add(smalList);
                        totalCount -= 50000;
                    }
                    List<ViewDebtorExport> lastList = data.Skip(i * 50000).Take(50000).ToList<ViewDebtorExport>();

                    bigData.Add(lastList);

                    return bigData;
                }

                bigData.Add(data.ToList<ViewDebtorExport>());

                return bigData;
            }
            catch (Exception e)
            {
                return new List<ViewDebtorExport>();
            }
            finally
            {
                Container.Release(service);
            }
        }

        public override ReportStreamResult ExportData(BaseParams baseParams)
        {
            IReportGenerator reportGenerator = Container.Resolve<IReportGenerator>("XlsIoGenerator");

            try
            {
                var dataList = GetExportData(baseParams);
                var zip = new Ionic.Zip.ZipFile("Export", Encoding.UTF8);
                int i = 0;

                foreach (var item in dataList)
                {
                    i++;
                    IDataExportReport dataExportReport = Container.Resolve<IDataExportReport>("DataExportReport.PrintForm", 
                        new Arguments {{ "Data", item },  { "BaseParams", baseParams}});
                    
                    ReportParams reportParams = new ReportParams();
                    dataExportReport.PrepareReport(reportParams);
                    Stream template = dataExportReport.GetTemplate();
                    MemoryStream memoryStream = new MemoryStream();
                    reportGenerator.Open(template);
                    reportGenerator.Generate(memoryStream, reportParams);
                    memoryStream.Seek(0L, SeekOrigin.Begin);
                    var report = new ReportStreamResult(memoryStream, "export.xlsx");

                    zip.AddEntry(string.Format("ExportList {0}.xlsx", i), memoryStream);
                }
                MemoryStream zipStream = new MemoryStream();
                zip.Save(zipStream);

                return new ReportStreamResult(zipStream, "export.zip");
            }
            finally
            {
                Container.Release(reportGenerator);
            }
        }
    }
}