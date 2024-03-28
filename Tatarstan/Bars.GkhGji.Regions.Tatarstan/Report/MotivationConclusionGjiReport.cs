namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Tatarstan.Properties;
    using Bars.GkhGji.Report;

    public class MotivationConclusionGjiReport : GjiBaseStimulReport
    {
        public MotivationConclusionGjiReport()
            : base(new ReportTemplateBinary(Resources.WarningDoc))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        protected long DocumentId { get; set; }

        protected override string CodeTemplate
        {
            get { return "MotivationConclusion"; }
            set { }
        }

        public override string Id => "MotivationConclusion";

        public override string CodeForm => "MotivationConclusion";

        public override string Name => "Мотивировочное заключение";

        public override string Description => "Мотивировочное заключение";

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
                    Name = "MotivationConclusion",
                    Code = "MotivationConclusion",
                    Description = "Мотивировочное заключение",
                    Template = Resources.WarningDoc
                },
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["ИдентификаторДокументаГЖИ"] = this.DocumentId.ToString();
        }
    }
}