using Bars.B4.DataAccess;
using Bars.B4.IoC;
using Bars.B4.Modules.Reports;
using Bars.B4.Utils;
using Bars.Gkh.Report;
using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
using Bars.GkhGji.Regions.Tatarstan.Enums;
using Bars.GkhGji.Regions.Tatarstan.Properties;
using System.Collections.Generic;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using Bars.B4.Modules.Analytics.Reports.Enums;

    public class PreventiveActionVisitNotificationReport : GkhBaseStimulReport
    {
        public PreventiveActionVisitNotificationReport()
            : base(new ReportTemplateBinary(Resources.PreventiveActionVisitNotification))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        public override string Id => "PreventiveActionVisitNotification";

        public override string CodeForm => "PreventiveActionVisitNotification";

        public override string Name => "Уведомление о проведении визита";

        public override string Description => "Уведомление о проведении визита";

        public long DocumentId { get; set; }

        protected override string CodeTemplate { get; set; }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Уведомление о проведении визита",
                    Code = "PreventiveActionVisitNotification",
                    Description = "Уведомление о проведении визита",
                    Template = Resources.PreventiveActionVisitNotification
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
