using Bars.B4.Modules.Reports;
using Bars.B4.Utils;
using Bars.Gkh.Report;
using Bars.GkhGji.Regions.Tatarstan.Properties;
using System.Collections.Generic;

namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using Bars.B4.Modules.Analytics.Reports.Enums;

    public class PreventiveActionTaskConsultationReport : GkhBaseStimulReport
    {
        public PreventiveActionTaskConsultationReport()
            : base(new ReportTemplateBinary(Resources.PreventiveActionTaskConsultation))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        public override string Id => "PreventiveActionTaskConsultation";

        public override string CodeForm => "PreventiveActionTaskConsultation";

        public override string Name => "Лист консультирования";

        public override string Description => "Печать листа консультирования";

        public long DocumentId { get; set; }

        protected override string CodeTemplate { get; set; }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Лист консультирования",
                    Code = "PreventiveActionTaskConsultation",
                    Description = "Печать листа консультирования",
                    Template = Resources.PreventiveActionTaskConsultation
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
