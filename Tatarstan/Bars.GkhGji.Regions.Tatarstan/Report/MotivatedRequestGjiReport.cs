namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Tatarstan.Properties;
    using Bars.GkhGji.Report;

    public class MotivatedRequestGjiReport : GjiBaseStimulReport
    {
        public MotivatedRequestGjiReport()
            : base(new ReportTemplateBinary(Resources.MotivatedRequest))
        {
            this.ExportFormat = StiExportFormat.Word2007;
        }

        protected long DocumentId { get; set; }

        protected override string CodeTemplate
        {
            get
            {

                return "MotivatedRequest";
            }
            set { }
        }

        public override string Id
        {
            get { return "MotivatedRequest"; }
        }

        public override string CodeForm
        {
            get { return "MotivatedRequest"; }
        }

        public override string Name
        {
            get { return "Мотивированный запрос"; }
        }

        public override string Description
        {
            get { return "Мотивированный запрос"; }
        }

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
                    Name = "MotivatedRequest",
                    Code = "MotivatedRequest",
                    Description = "Мотивированный запрос",
                    Template = Resources.MotivatedRequest
                },
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["ИдентификаторДокументаГЖИ"] = this.DocumentId.ToString();
        }
    }
}