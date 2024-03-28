namespace Bars.Gkh.RegOperator.Report.ChesImport
{
    using System.Collections.Generic;
    using System.Reflection;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Report;


    /// <summary>
    /// Базовый класс отчёта контрольных проверок импорта ЧЭС
    /// </summary>
    public abstract class ChesImportReport : GkhBaseStimulReport
    {
        private long periodId;

        /// <inheritdoc />
        public override string Id => this.GetType().FullName;

        /// <inheritdoc />
        public override string Description => this.Name;

        /// <inheritdoc />
        public override StiExportFormat ExportFormat => StiExportFormat.Excel2007;

        /// <inheritdoc />
        public override string CodeForm => "ChesReport";

        /// <inheritdoc />
        protected override string CodeTemplate
        {
            get { return this.Id; }
            set { }
        }

        /// <inheritdoc />
        protected ChesImportReport(IReportTemplate reportTemplate)
            : base(reportTemplate)
        {
        }

        /// <inheritdoc />
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.periodId = userParamsValues.GetValue<long>("periodId");
        }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["periodId"] = this.periodId.ToString();
        }

        /// <inheritdoc/>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = this.Id,
                    Description = this.Description,
                    Name = this.Name,
                    Template = this.Template
                }
            };
        }

        /// <inheritdoc />
        public override string GetFileName()
        {
            return this.Name;
        }

        /// <summary>
        /// Шаблон
        /// </summary>
        protected abstract byte[] Template { get; }
    }
}