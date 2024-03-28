namespace Bars.Gkh.Gis.Reports.Reports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using B4;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;

    using Castle.Windsor;

    public abstract class StimulReport : IReportGeneratorFileName, IReportGeneratorMimeType, IGeneratedPrintForm
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>Формат печатной формы</summary>
        public virtual StiExportFormat ExportFormat { get; set; }

        /// <summary>Компилируемый отчет</summary>
        protected Dictionary<string, object> ReportParams { get; set; }

        protected List<MetaData> DataSources { get; set; }

        private Stream template;

        /// <summary> IReportGenerator.Open </summary>
        public virtual void Open(Stream reportTemplate)
        {
            this.ReportParams = new Dictionary<string, object>();
            this.DataSources = new List<MetaData>();

            this.template = reportTemplate;
        }

        public virtual void Generate(Stream result, ReportParams reportParams)
        {
            var container = ApplicationContext.Current.Container;

            var dataSources = this.DataSources.Select(x =>
                new DataSource(new CustomSingleDataProvider<object>(x.SourceName, x.Data)));

            var report = new CustomReport(dataSources,
                Array.Empty<IParam>(),
                "StimulReport",
                this.GetType().Name,
                this.template);

            var remoteReportService = container.Resolve<IRemoteReportService>();
            using (container.Using(remoteReportService))
            {
                var stream = remoteReportService.Generate(report, this.template, new BaseParams(), PrintFormat(this.ExportFormat), this.ReportParams);

                stream.CopyTo(result);
                result.Seek(0, SeekOrigin.Begin);
            }
        }
        
        private static ReportPrintFormat PrintFormat(StiExportFormat stiExportFormat)
        {
            return stiExportFormat switch
            {
                StiExportFormat.Pdf => ReportPrintFormat.pdf,
                StiExportFormat.Xps => ReportPrintFormat.xps,
                StiExportFormat.Text => ReportPrintFormat.text,
                StiExportFormat.Excel => ReportPrintFormat.xls,
                StiExportFormat.Excel2007 => ReportPrintFormat.xlsx,
                StiExportFormat.Word2007 => ReportPrintFormat.docx,
                StiExportFormat.Csv => ReportPrintFormat.csv,
                _ => throw new ArgumentOutOfRangeException(nameof(stiExportFormat), stiExportFormat, null)
            };
        }


        public virtual MemoryStream GetGeneratedReport()
        {
            var reportParams = new ReportParams();

            Open(GetTemplate());
            PrepareReport(reportParams);

            var ms = new MemoryStream();
            Generate(ms, reportParams);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public virtual string GetFileName()
        {
            string extention;
            switch (ExportFormat)
            {
                case StiExportFormat.Excel2007: extention = ".xlsx"; break;
                case StiExportFormat.Word2007: extention = ".docx"; break;
                case StiExportFormat.Pdf: extention = ".pdf"; break;
                case StiExportFormat.Ppt2007: extention = ".ppt"; break;
                case StiExportFormat.Odt: extention = ".odt"; break;
                case StiExportFormat.Text: extention = ".txt"; break;
                case StiExportFormat.ImagePng: extention = ".png"; break;
                case StiExportFormat.ImageSvg: extention = ".svg"; break;
                case StiExportFormat.ImageEmf: extention = ".emf"; break;
                case StiExportFormat.ImageJpeg: extention = ".jpg"; break;
                case StiExportFormat.Html: extention = ".html"; break;
                case StiExportFormat.Html5: extention = ".html"; break;
                case StiExportFormat.HtmlDiv: extention = ".html"; break;
                case StiExportFormat.HtmlSpan: extention = ".html"; break;
                case StiExportFormat.HtmlTable: extention = ".html"; break;
                case StiExportFormat.RtfWinWord: extention = ".rtf"; break;
                default: extention = ".bin"; break;
            }

            return "report" + extention;
        }

        /// <summary> MIME type </summary>
        public virtual string GetMimeType()
        {
            return MimeTypeHelper.GetMimeType(Path.GetExtension(GetFileName()));
        }

        public abstract Stream GetTemplate();

        public abstract void PrepareReport(ReportParams reportParams);

        public abstract string Name { get; }

        public virtual void SetUserParams(BaseParams baseParams)
        {
            
        }

        public abstract string Desciption { get; }

        public abstract string GroupName { get; }

        public abstract string ParamsController { get; }

        public abstract string RequiredPermission { get; }
    }
}
