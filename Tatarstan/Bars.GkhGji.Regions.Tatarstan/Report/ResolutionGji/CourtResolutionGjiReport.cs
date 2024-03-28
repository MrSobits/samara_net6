namespace Bars.GkhGji.Regions.Tatarstan.Report.ResolutionGji
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Tatarstan.Properties;
    using Bars.GkhGji.Report;

    /// <inheritdoc />
    public sealed class CourtResolutionGjiReport : GjiBaseStimulReport
    {
        public CourtResolutionGjiReport()
            : base(new ReportTemplateBinary(Resources.BlockGJI_CourtResolution))
        {
            this.ExportFormat = StiExportFormat.Word2007;
        }

        private long DocumentId { get; set; }

        protected override string CodeTemplate
        {
            get => "BlockGJI_CourtResolution";
            set { }
        }

        public override string Id => "CourtResolution";

        public override string CodeForm => "CourtResolution";

        public override string Name => "Постановление суда";

        public override string Description => "Постановление суда";

        public override string Permission => "Reports.GJI.CourtResolutionReport";

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
                    Name = "CourtResolutionGJI",
                    Code = "BlockGJI_CourtResolution",
                    Description = "Постановление суда",
                    Template = Resources.BlockGJI_CourtResolution
                }
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["ИдентификаторДокументаГЖИ"] = this.DocumentId.ToString();
        }
    }
}