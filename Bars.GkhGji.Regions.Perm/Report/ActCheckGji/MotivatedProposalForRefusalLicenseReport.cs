namespace Bars.GkhGji.Regions.Perm.Report.ActCheckGji
{
    using Bars.B4.Modules.Reports;
    using Gkh.Report;
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;

    /// <summary>
    /// Отчет Мотивированное предложение об отказе в выдачи лицензии
    /// </summary>
    public class MotivatedProposalForRefusalLicenseReport : GkhBaseStimulReport
    {
        public MotivatedProposalForRefusalLicenseReport() : base(new ReportTemplateBinary(Properties.Resources.MotivatedProposalForRefusalLicenseReport))
        {  
        }

        private long actCheckId;

        public override string Id => nameof(MotivatedProposalForRefusalLicenseReport);

        public override string CodeForm => "ActCheck";

        public override string Description => "Мотивированное предложение об отказе в выдачи лицензии";

        public override string Name => "Мотивированное предложение об отказе в выдачи лицензии";

        public override string Permission => "Reports.GJI.MotivatedProposalForRefusalLicenseReport";

        protected override string CodeTemplate
        {
            get { return this.Id; }
            set { }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = this.Id,
                    Description = this.Description,
                    Name = this.Name,
                    Template = Properties.Resources.MotivatedProposalForRefusalLicenseReport
                }
            };
        }

        public override StiExportFormat ExportFormat => StiExportFormat.Word2007;

        public override void PrepareReport(ReportParams reportParams)
        {
            this.ReportParams["ИдентификаторДокументаГЖИ"] = this.actCheckId.ToString();
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.actCheckId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }
    }
}