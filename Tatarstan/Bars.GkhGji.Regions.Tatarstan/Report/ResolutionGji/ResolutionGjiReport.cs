namespace Bars.GkhGji.Regions.Tatarstan.Report.ResolutionGji
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Properties;
    using Bars.GkhGji.Report;

    public class ResolutionGjiReport : GjiBaseStimulReport
    {
        public ResolutionGjiReport()
            : base(new ReportTemplateBinary(Resources.BlockGJI_Resolution1))
        {
            this.ExportFormat = StiExportFormat.Word2007;
        }

        protected long DocumentId { get; set; }

        protected override string CodeTemplate
        {
            get
            {
                var documentGjiDomain = this.Container.ResolveDomain<DocumentGji>();

                var typeBase = documentGjiDomain.GetAll().Where(x => x.Id == this.DocumentId).Select(x => x.Inspection.TypeBase).FirstOrDefault();

                if (typeBase == TypeBase.ProtocolMvd)
                {
                    return "BlockGJI_Resolution_ProtocolMvd";
                }
                return "BlockGJI_Resolution";
            }
            set { }
        }

        public override string Id
        {
            get { return "Resolution"; }
        }

        public override string CodeForm
        {
            get { return "Resolution"; }
        }

        public override string Name
        {
            get { return "Постановление"; }
        }

        public override string Description
        {
            get { return "Постановление"; }
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
                    Name = "ResolutionGJI",
                    Code = "BlockGJI_Resolution",
                    Description = "Постановление о назначении",
                    Template = Resources.BlockGJI_Resolution1
                },
                new TemplateInfo
                {
                    Name = "ResolutionGJI",
                    Code = "BlockGJI_Resolution_ProtocolMvd",
                    Description = "Постановление протокола МВД",
                    Template = Resources.BlockGJI_Resolution_ProtocolMvd
                }
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["ИдентификаторДокументаГЖИ"] = this.DocumentId.ToString();
        }
    }
}