﻿namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Tatarstan.Properties;

    public class ActActionIsolatedReport : GkhBaseStimulReport
    {
        public ActActionIsolatedReport()
            : base(new ReportTemplateBinary(Resources.ActActionIsolated))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        protected override string CodeTemplate { get; set; }

        protected long DocumentId { get; set; }

        public override string Extention => "mrt";

        public override string Id => "ActActionIsolated";

        public override string CodeForm => "ActActionIsolated";

        public override string Name => "Акт КНМ без взаимодействия";

        public override string Description => "Акт КНМ без взаимодействия";


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
                    Name = "Акт КНМ без взаимодействия",
                    Code = "ActActionIsolated",
                    Description = "Акт КНМ без взаимодействия",
                    Template = Resources.ActActionIsolated
                }
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["Id"] = this.DocumentId.ToString();
        }
    }
}