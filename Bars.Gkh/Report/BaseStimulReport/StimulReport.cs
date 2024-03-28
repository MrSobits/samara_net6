namespace Bars.Gkh.StimulReport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using B4.Application;
    using B4.Modules.Reports;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators.Models;
    using Bars.B4.Utils;

	/// <summary>
    /// Базовый отчет Stimulsoft
    /// Пример использования:
    /// 
    /// // Создаем отчет и передаем ему параметры отчета
    /// var report = new Report { Container = Container };
    ///
    /// // Устанавливаем формат печати
    /// report.SetExportFormat(new PrintFormExportFormat { Id = (int)StiExportFormat.Pdf });
    /// 
    /// // открываем шаблон
    /// var template = report.GetTemplate();
    /// report.Open(template);
    /// 
    /// // Запускаем формирование и получениие результата в виде MemoryStream
    /// var result = new MemoryStream();
    /// report.PrepareReport(new ReportParams());
    /// report.Generate(result, new ReportParams());
    /// result.Seek(0, SeekOrigin.Begin);
    /// 
    /// // Готовый отчет
    /// return result;
    /// </summary>
    public class StimulReport : IReportGenerator, IReportGeneratorFileName, IReportGeneratorMimeType, IExportablePrintForm
    {
        /// <summary>Формат печатной формы</summary>
        public virtual StiExportFormat ExportFormat { get; set; }

        /// <summary>Настройки экспорта (для каждого формата свой производный тип)</summary>
        public virtual Dictionary<string, string> ExportSettings { get; set; }

        protected Dictionary<string, object> ReportParams;

        protected List<MetaData> DataSources;

        private Stream template;

        /// <summary> IReportGenerator.Open </summary>
        public virtual void Open(Stream reportTemplate)
        {
            this.ReportParams = new Dictionary<string, object>();
            this.DataSources = new List<MetaData>();

            this.template = reportTemplate;
        }

		/// <summary>
		/// Сгенерировать отчет
		/// </summary>
		/// <param name="result">Результат</param>
		/// <param name="reportParams">Параметры отчета</param>
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
                var stream = remoteReportService.Generate(report, this.template, new BaseParams(), PrintFormat(this.ExportFormat), this.ReportParams, ExportSettings);

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

		/// <summary>
		/// Получить сгенерированный отчет
		/// </summary>
		/// <returns></returns>
        public virtual MemoryStream GetGeneratedReport()
        {
            var reportParams = new ReportParams();
            var printForm = this as IBaseReport;
            if (printForm != null)
            {
                // Если кто-то захочет поменять порядок вызовов метода Open и PrepareREport
                // то такое делать ненадо
                // поскольку для Sti,ula сначала нужно всебя затянуть шаблон, а только потом заполнять словарь Report["параметр"]
                Open(printForm.GetTemplate());
                printForm.PrepareReport(reportParams);
            }

            var ms = new MemoryStream();
            Generate(ms, reportParams);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

		/// <summary>
		/// Получить имя файла
		/// </summary>
		/// <returns></returns>
        public virtual string GetFileName()
        {
            var extention = string.Empty;
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
        /// <summary> IExportablePrintForm.GetExportFormats() </summary>
        public virtual IList<PrintFormExportFormat> GetExportFormats()
        {
            return new[]
            {
                new PrintFormExportFormat { Id = (int)StiExportFormat.Excel2007, Name = "MS Excel 2007"       },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Word2007,  Name = "MS Word 2007"        },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Pdf,       Name = "Adobe Acrobat"       },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Ppt2007,   Name = "MS Power Point"      },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Odt,       Name = "OpenDocument Writer" },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Text,      Name = "Текст (TXT)"         },
                new PrintFormExportFormat { Id = (int)StiExportFormat.ImagePng,  Name = "Изображение (PNG)"   },
                new PrintFormExportFormat { Id = (int)StiExportFormat.ImageSvg,  Name = "Изображение (SVG)"   },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Html,      Name = "Веб страница (Html)" }
            };
        }
        /// <summary> IExportablePrintForm.SetExportFormat() </summary>
        public virtual void SetExportFormat(PrintFormExportFormat format)
        {
            ExportFormat = (StiExportFormat)format.Id;
        }
    }
}