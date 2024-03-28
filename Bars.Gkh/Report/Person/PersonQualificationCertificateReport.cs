namespace Bars.Gkh.Report
{
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Properties;
    using Slepov.Russian.Morpher;
     using Bars.B4.Modules.Analytics.Reports.Enums;
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    public class PersonQualificationCertificateReport : GkhBaseStimulReport
    {
        private long certificateId;

        protected Person Person { get; set; }

        public IDomainService<PersonQualificationCertificate> PersonQualificationCertificateDomain { get; set; }

        private Склонятель _morpher;

        protected Склонятель GetMorpher()
        {
            return _morpher ?? (_morpher = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ=="));
        }

        public PersonQualificationCertificateReport() : base(new ReportTemplateBinary(Resources.PersonQualificationCertificateReport))
        {
        }

        public override string Permission
        {
            get { return "Reports.GKH.PersonQualificationCertificateReport"; }
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var certificate = this.PersonQualificationCertificateDomain.Get(this.certificateId);

            this.ReportParams["НомерКА"] = certificate.Number.ToString();

            var morpher = GetMorpher();
            var surname = morpher.Проанализировать(Person.Surname ?? "");
            var name = morpher.Проанализировать(Person.Name ?? "");
            var patronymic = morpher.Проанализировать(Person.Patronymic ?? "");

            this.ReportParams["ФИОРодП"] = "{0} {1} {2}".FormatUsing(
                surname != null ? surname.Дательный : "",
                name != null ? name.Дательный : "",
                patronymic != null ? patronymic.Дательный : "");

        }

        public override string Id
        {
            get { return "PersonQualificationCertificateReport"; }
        }

        public override string CodeForm
        {
            get { return "Person"; }
        }

        public override string Name
        {
            get { return "Квалификационный аттестат"; }
        }

        public override string Description
        {
            get { return "Квалификационный аттестат"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.certificateId = userParamsValues.GetValue<object>("PersonId").ToLong();
        }

        public override string Extention
        {
            get { return "mrt"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
            set { }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "PersonQualificationCertificateReport",
                    Description = "Квалификационный аттестат",
                    Name = "Квалификационный аттестат",
                    Template = Resources.PersonQualificationCertificateReport
                }
            };
        }
    }
}
