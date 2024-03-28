namespace Bars.GkhGji.Regions.Tatarstan.Report
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Tatarstan.Properties;

    public class TatProtocolGjiReport : GkhBaseStimulReport
    {
        public TatProtocolGjiReport()
            : base(new ReportTemplateBinary(Resources.TatProtocolGji))
        {
            base.ExportFormat = StiExportFormat.Word2007;
        }

        protected override string CodeTemplate
        {
            get { return "TatProtocolGji"; }
            set { }
        }

        protected long DocumentId { get; set; }

        public override string Extention => "mrt";

        public override string Id => "TatProtocolGji";

        public override string CodeForm => "TatProtocolGji";

        public override string Permission => "Reports.GJI.TatProtocolGjiReport";

        public override string Name => "Протокол ГЖИ";

        public override string Description => "Протокол ГЖИ РТ по ст. 20.6.1 КОАП РФ";


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
                    Name = "Протокол ГЖИ",
                    Code = "TatProtocolGji",
                    Description = "Протокол ГЖИ РТ по ст. 20.6.1 КОАП РФ",
                    Template = Resources.TatProtocolGji
                }
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["ИдентификаторДокументаГЖИ"] = this.DocumentId.ToString();
        }
    }
}
