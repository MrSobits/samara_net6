namespace Bars.Gkh.Report
{
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Properties;
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    public class MotivatedProposalForLicensingReport : GkhBaseStimulReport
    {
        private long requestId;

        public IDomainService<ManOrgLicenseRequest> RequestDomain { get; set; }

        public IDbConfigProvider dbConfigProvider { get; set; }

        public MotivatedProposalForLicensingReport() : base(new ReportTemplateBinary(Resources.MotivatedProposalForLicensing))
        {
        }


        public override string Permission
        {
            get { return "Reports.GKH.MotivatedProposalForLicensingReport"; }
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var request = this.RequestDomain.Get(this.requestId);

            this.ReportParams["ИдентификаторДокументаГЖИ"] = request.Id.ToString();
            this.ReportParams["СтрокаПодключениякБД"] = this.dbConfigProvider.ConnectionString;
        }

        public override string Id
        {
            get { return "MotivatedProposalForLicensingReport"; }
        }

        public override string CodeForm
        {
            get { return "ManOrgLicense"; }
        }

        public override string Name
        {
            get { return "Мотивированное предложение о выдаче лицензии"; }
        }

        public override string Description
        {
            get { return "Мотивированное предложение о выдаче лицензии"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.requestId = userParamsValues.GetValue<object>("RequestId").ToLong();
        }

        public override string Extention
        {
            get { return "mrt"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat => StiExportFormat.Word2007;

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "MotivatedProposalForLicensingReport",
                    Description = "Мотивированное предложение о выдаче лицензии",
                    Name = "Мотивированное предложение о выдаче лицензии",
                    Template = Resources.MotivatedProposalForLicensing
                }
            };
        }
    }
}