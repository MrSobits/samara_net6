namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Tatarstan.Properties;

    public class MotivatedPresentationReport : GkhBaseStimulReport
    {
        /// <inheritdoc />
        public MotivatedPresentationReport()
            : base(new ReportTemplateBinary(Resources.MotivatedPresentationActionIsolated))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }
        
        protected long DocumentId { get; set; }
        
        public override string Extention => "mrt";

        protected override string CodeTemplate { get; set; }

        public override string Id => "MotivatedPresentationActionIsolated";

        public override string CodeForm => "MotivatedPresentationActionIsolated";

        public override string Name => "Мотивированное представление";

        public override string Description => "Мотивированное представление";

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "MotivatedPresentationActionIsolated",
                    Code = "MotivatedPresentationActionIsolated",
                    Description = "Мотивированное представление",
                    Template = Resources.MotivatedPresentationActionIsolated
                },
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Id"] = this.DocumentId.ToString();
        }
    }
}