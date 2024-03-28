namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Tatarstan.Properties;

    public class PreventiveActionVisitSheetReport: GkhBaseStimulReport
    {
        public PreventiveActionVisitSheetReport()
            : base(new ReportTemplateBinary(Resources.PreventiveActionVisitSheet))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        protected override string CodeTemplate { get; set; }

        protected long DocumentId { get; set; }

        public override string Extention => "mrt";

        public override string Id => "PreventiveActionVisitSheet";

        public override string CodeForm => "PreventiveActionVisitSheet";

        public override string Name => "Лист визита";

        public override string Description => "Лист визита";

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
                    Name = "Лист визита",
                    Code = "PreventiveActionVisitSheet",
                    Description = "Лист визита",
                    Template = Resources.PreventiveActionVisitSheet
                }
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Id"] = this.DocumentId.ToString();
        }
    }
}