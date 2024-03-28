namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Contracts;

    /// <summary>
    /// 
    /// </summary>
    public class SpecialAccountReportReport : GkhBaseStimulReport
    {
        private long DocumentId { get; set; }
        /// <summary>
        /// Код шаблона
        /// </summary>
        protected override string CodeTemplate { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        public SpecialAccountReportReport() : base(new ReportTemplateBinary(Properties.Resources.SpecAccReport))
        {
        }
        /// <summary>
        /// Названия отчета
        /// </summary>
        public override string ReportGeneratorName
        {
            get { return "DocIoGenerator"; }
        }
        /// <summary>
        /// Установка параметров
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Pdf; }
        }
        /// <summary>
        /// Информация о шаблоне
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "SpecialAccountReportReport",
                            Name = "Отчет по спецсчетам",
                            Description = "Отчет по спецсчетам",
                            Template = Properties.Resources.SpecAccReport
                        }
                };
        }
        /// <summary>
        /// ID
        /// </summary>
        public override string Id
        {
            get { return "SpecialAccountReportReport"; }
        }
        /// <summary>
        /// Код
        /// </summary>
        public override string CodeForm
        {
            get { return "SpecialAccountReport"; }
        }
        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Отчет по спецсчетам"; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        public override string Description
        {
            get { return "Отчет по спецсчетам"; }
        }
        /// <summary>
        /// Подготовка данных для отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["rep_id"] = this.DocumentId;
          //  reportParams.SimpleReportParams["rep_id"] = this.DocumentId;  
        }
    }
}