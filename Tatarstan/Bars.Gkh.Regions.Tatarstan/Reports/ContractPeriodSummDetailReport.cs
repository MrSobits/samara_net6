namespace Bars.Gkh.Regions.Tatarstan.Reports
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Regions.Tatarstan.Properties;
    using Bars.Gkh.Report;

    /// <summary>
    /// Экспорт расщепления платежей
    /// </summary>
    public class ContractPeriodSummDetailReport : GkhBaseStimulReport
    {
        /// <summary>
        /// Код шаблона
        /// </summary>
        public static string Code => nameof(ContractPeriodSummDetailReport);

        /// <summary>
        /// Файл шаблона по умолчанию
        /// </summary>
        public static byte[] TemplateFile => Resources.ContractPeriodSummDetailReport;

        private string contractId;

        /// <inheritdoc/>
        public ContractPeriodSummDetailReport()
            : base(new ReportTemplateBinary(ContractPeriodSummDetailReport.TemplateFile))
        {
        }

        /// <inheritdoc/>
        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Id"] = this.contractId;
        }

        /// <inheritdoc/>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.contractId = userParamsValues.GetValue<string>("contractId");
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
                    Template = ContractPeriodSummDetailReport.TemplateFile
                }
            };
        }

        /// <inheritdoc/>
        public override string CodeForm => ContractPeriodSummDetailReport.Code;

        /// <inheritdoc/>
        public override string Name => "Начисления и распределения платежей";

        /// <inheritdoc/>
        public override string Description => "Расщепление платежей населения за коммунальные услуги по  договору ресурсоснабжения  по состоянию на месяц.год г.";

        /// <inheritdoc/>
        protected override string CodeTemplate { get; set; } = ContractPeriodSummDetailReport.Code;

        /// <inheritdoc/>
        public override string Id => ContractPeriodSummDetailReport.Code;

        /// <inheritdoc/>
        public override StiExportFormat ExportFormat => StiExportFormat.Excel2007;
    }
}