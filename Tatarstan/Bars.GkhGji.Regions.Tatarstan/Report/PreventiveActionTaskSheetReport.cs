using Bars.B4.Modules.Reports;
using Bars.B4.Utils;
using Bars.Gkh.Report;
using Bars.GkhGji.Regions.Tatarstan.Properties;
using System.Collections.Generic;

namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using Bars.B4.Modules.Analytics.Reports.Enums;

    public class PreventiveActionTaskSheetReport : GkhBaseStimulReport
    {
        public PreventiveActionTaskSheetReport()
            : base(new ReportTemplateBinary(Resources.PreventiveActionTaskSheet))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        public override string Id => "PreventiveActionTaskSheet";

        public override string CodeForm => "PreventiveActionTaskSheet";

        public override string Name => "Задание на проведение визита";

        public override string Description => "Задание на проведение визита";

        public long DocumentId { get; set; }

        protected override string CodeTemplate { get; set; }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Задание на проведение визита",
                    Code = "PreventiveActionTaskSheet",
                    Description = "Задание на проведение визита",
                    Template = Resources.PreventiveActionTaskSheet
                }
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Id"] = this.DocumentId.ToString();
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }
    }
}
