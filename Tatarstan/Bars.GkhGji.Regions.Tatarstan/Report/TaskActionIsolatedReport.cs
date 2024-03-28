namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Tatarstan.Properties;

    public class TaskActionIsolatedReport: GkhBaseStimulReport
    {
        public TaskActionIsolatedReport()
            : base(new ReportTemplateBinary(Resources.TaskActionIsolated))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        protected override string CodeTemplate { get; set; }

        protected long DocumentId { get; set; }

        public override string Extention => "mrt";

        public override string Id => "TaskAction";

        public override string CodeForm => "TaskAction";

        public override string Name => "Задание на проведение КНМ без взаимодействия с контролируемым лицом";

        public override string Description => "Задание на проведение КНМ без взаимодействия с контролируемым лицом";


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
                    Name = "Задание на проведение КНМ без взаимодействия с контролируемым лицом",
                    Code = "TaskActionIsolated",
                    Description = "Задание на проведение КНМ без взаимодействия с контролируемым лицом",
                    Template = Resources.TaskActionIsolated
                }
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Id"] = this.DocumentId.ToString();
        }
    }
}
