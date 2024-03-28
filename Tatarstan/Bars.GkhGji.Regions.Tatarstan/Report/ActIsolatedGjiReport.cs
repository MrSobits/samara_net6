namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Tatarstan.Properties;
    using Bars.GkhGji.Report;

    public class ActIsolatedGjiReport : GjiBaseStimulReport
    {
        public ActIsolatedGjiReport()
            : base(new ReportTemplateBinary(Resources.ActIsolated))
        {
            this.ExportFormat = StiExportFormat.Word2007;
        }

        protected long DocumentId { get; set; }

        protected override string CodeTemplate
        {
            get
            {
                return "ActIsolated";
            }
            set { }
        }

        public override string Id
        {
            get { return "ActIsolated"; }
        }

        public override string CodeForm
        {
            get { return "ActIsolated"; }
        }

        public override string Name
        {
            get { return "Акт без взаимодействия"; }
        }

        public override string Description
        {
            get { return "Акт без взаимодействия"; }
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
                    Name = "ActIsolated",
                    Code = "ActIsolated",
                    Description = "Акт без взаимодействия",
                    Template = Resources.ActIsolated
                },
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["ИдентификаторДокументаГЖИ"] = this.DocumentId.ToString();
        }
    }
}