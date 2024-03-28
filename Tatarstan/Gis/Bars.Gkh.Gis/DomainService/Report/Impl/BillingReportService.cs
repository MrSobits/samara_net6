namespace Bars.Gkh.Gis.DomainService.Report.Impl
{
    using System.IO;
    using B4;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Castle.Windsor;
    using Report;

    public class BillingReportService : IBillingReportService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получение отчета
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult GetReport(BaseParams baseParams)
        {
            var exportFormat = baseParams.Params.GetAs<int>("ExportFormat");
            //var reportParams = baseParams.Params.GetAs<List<object>>("ReportParams");

            //Получение объекта отчета
            var printFormObject = new PrintForm
            {
                Name = baseParams.Params.GetAs<string>("ReportName"),
                ClassName = baseParams.Params.GetAs<string>("ClassName")
            };
            var reportObject = Container.Resolve<IBillingReport>(printFormObject.ClassName);
            reportObject.Open(reportObject.GetTemplate());
            reportObject.SetUserParams(baseParams);
            reportObject.SetExportFormat(exportFormat);

            //Подготавливаем отчет и передаем ему параметры
            reportObject.PrepareReport(new ReportParams
            {
                SimpleReportParams = new SimpleReportParams()
                //{
                //    СписокПараметров = (reportParams != null && reportParams.Any() && reportParams[0] != null) ?
                //        ((DynamicDictionary)reportParams[0]).ToDictionary(x => x.Key, x => x.Value) : null
                //}
            });

            var result = new MemoryStream();
            reportObject.Generate(result);

            //Получаем расширение отчета
            var extension = getFormatExtension(exportFormat);
            return new BaseDataResult(result)
            {
                Message = extension != null ? reportObject.Name + "." + extension : ""
            };
        }

        /// <summary>
        /// Получение списка доступных форматов для отчета
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult GetExportFormatList(BaseParams baseParams)
        {
            //Получение объекта отчета
            var printFormObject = new PrintForm
            {
                Name = baseParams.Params.GetAs<string>("ReportName"),
                ClassName = baseParams.Params.GetAs<string>("ClassName")
            };
            var report = Container.Resolve<IBillingReport>(printFormObject.ClassName);

            var formatList = report.GetExportFormats();
            return new BaseDataResult(formatList);
        }

        /// <summary>
        /// Получение расширения формата
        /// </summary>
        /// <param name="exportFormat"></param>
        /// <returns></returns>
        private string getFormatExtension(int exportFormat)
        {
            switch (exportFormat)
            {
                case (int)StiExportFormat.Pdf:
                    return "pdf";
                case (int)StiExportFormat.Excel:
                    return "xls";
                case (int)StiExportFormat.Excel2007:
                    return "xlsx";
                case (int)StiExportFormat.Word2007:
                    return "docx";
                case (int)StiExportFormat.Csv:
                    return "csv";
                case (int)StiExportFormat.Xml:
                    return "xml";
                case (int)StiExportFormat.Rtf:
                    return "rtf";
                case (int)StiExportFormat.Text:
                    return "txt";
                default:
                    return null;
            }
        }
    }
}