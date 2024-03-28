namespace Bars.Gkh.Report
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Properties;

    /// <summary>
    /// Экспорт Мониторинга жилищного фонда
    /// </summary>
    public class HousingFundMonitoringInfoReport : GkhBaseStimulReport
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public HousingFundMonitoringInfoReport()
             : base(new ReportTemplateBinary(HousingFundMonitoringInfoReport.TemplateFile))
        {
        }

        private string periodId;

        /// <summary>
        /// Файл шаблона по умолчанию
        /// </summary>
        public static byte[] TemplateFile => Resources.HousingFundMonitoringInfoReport;

        /// <summary>
        /// Код шаблона
        /// </summary>
        public static string Code => nameof(HousingFundMonitoringInfoReport);

        /// <inheritdoc/>
        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Id"] = this.periodId;
        }

        /// <inheritdoc/>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.periodId = userParamsValues.GetValue<string>("periodId");
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
                    Template = HousingFundMonitoringInfoReport.TemplateFile
                }
            };
        }

        /// <inheritdoc/>
        public override string Name => "Мониторинг жилищного фонда";

        /// <inheritdoc/>
        public override string Description => "Мониторинг жилищного фонда";

        /// <inheritdoc/>
        protected override string CodeTemplate { get; set; } = HousingFundMonitoringInfoReport.Code;

        /// <inheritdoc/>
        public override string Id => HousingFundMonitoringInfoReport.Code;

        /// <inheritdoc/>
        public override string CodeForm => HousingFundMonitoringInfoReport.Code;

        /// <inheritdoc/>
        public override StiExportFormat ExportFormat => StiExportFormat.Excel2007;
    }
}