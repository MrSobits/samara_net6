namespace Bars.Gkh.Report.Licensing
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Properties;
    using Bars.Gkh.Report;

    public class LicenseApplicationPrimaryReport : GkhBaseStimulReport
    {
        private long _requestId;

        private ManOrgLicenseRequest Request { get; set; }

        public IDomainService<ManOrgLicenseRequest> RequestDomain { get; set; }

        public IDomainService<ManOrgRequestPerson> RequestPersonDomain { get; set; }

        public IDomainService<PersonQualificationCertificate> CertificateDomain { get; set; }

        public IDomainService<PersonPlaceWork> PlaceWorkDomain { get; set; }

        public LicenseApplicationPrimaryReport() : base(new ReportTemplateBinary(Resources.LicenseApplicationPrimary))
        {
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var persons = RequestPersonDomain.GetAll().Where(x => x.LicRequest.Id == _requestId).Select(x => x.Person).ToArray();
            var personsIds = persons.Select(x => x.Id).ToArray();
            var certificates = CertificateDomain.GetAll().Where(x => personsIds.Contains(x.Person.Id)).Select(x => new
            {
                x.Number,
                x.IssuedDate
            }).ToArray().Select(x =>
            {
                var result = "№ " + x.Number;
                if (x.IssuedDate.HasValue)
                {
                    result += " от " + x.IssuedDate.Value.ToShortDateString();
                }

                return result;
            });

            var certificateString = string.Join(", ", certificates);

             var placeWorkDict = PlaceWorkDomain.GetAll()
                .Where(x => x.StartDate.HasValue)
                .Where(x => personsIds.Contains(x.Person.Id))
                .Select(x => new
                {
                    personId = x.Person.Id,
                    position = x.Position.Name,
                    date = x.StartDate.Value
                })
                .AsEnumerable()
                .GroupBy(x => x.personId)
                .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.date).Select(z => z.position).First());


            this.ReportParams["СоискательЛицензииСокр"] = Request.Contragent.ShortName;
            this.ReportParams["СоискательЛицензииПолн"] = Request.Contragent.Name;
            this.ReportParams["ОПФ"] = Request.Contragent.OrganizationForm.Name;
            this.ReportParams["ФактАдрес"] = Request.Contragent.FactAddress;
            this.ReportParams["ОГРН"] = Request.Contragent.Ogrn;
            this.ReportParams["РеквизитыЕГРЮЛ"] = Request.Contragent.OgrnRegistration;
            this.ReportParams["ИНН"] = Request.Contragent.Inn;
            this.ReportParams["КвалАттестат"] = certificateString;
            this.ReportParams["Сайт"] = Request.Contragent.OfficialWebsite;
            this.ReportParams["УплатаГосПошлины"] = Request.ConfirmationOfDuty;
            this.ReportParams["Телефон"] = Request.Contragent.Phone;
            this.ReportParams["ЭлПочта"] = Request.Contragent.Email;
            this.ReportParams["ФИОСоискателя"] = string.Join(", ", persons.Select(x => x.FullName));
            this.ReportParams["ДолжностьСоискателя"] = string.Join(", ", persons.Select(x => placeWorkDict.ContainsKey(x.Id) ? placeWorkDict[x.Id] : string.Empty));
            this.ReportParams["РеквизитыДокументаОПостановкеНаУчет"] =
                string.Format("Свидетельство о постановке на учет юридического лица серии {0} № {1} от {2}., выданное {3}",
                    Request.Contragent.TaxRegistrationSeries, 
                    Request.Contragent.TaxRegistrationNumber,
                    Request.Contragent.TaxRegistrationDate.HasValue
                        ? " от " + Request.Contragent.TaxRegistrationDate.Value.ToShortDateString()
                        : string.Empty,
                    Request.Contragent.TaxRegistrationIssuedBy);
        }

        public override string Id
        {
            get { return "LicenseApplicationPrimary"; }
        }

        public override string CodeForm
        {
            get { return "ManOrgLicenseRequest"; }
        }

        public override string Permission
        {
            get { return "Reports.GKH.ManOrgLicenseRequestLicenseApplicationPrimary"; }
        }

        public override string Name
        {
            get { return "Заявление о предоставлении лицензии (первичное обращение)"; }
        }

        public override string Description
        {
            get { return "Заявление о предоставлении лицензии (первичное обращение)"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _requestId = userParamsValues.GetValue<long>("RequestId");
            Request = RequestDomain.Get(_requestId);
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat => StiExportFormat.Word2007;

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "LicenseApplicationPrimary",
                    Description = "Заявление о предоставлении лицензии (первичное обращение)",
                    Name = "LicenseApplicationPrimary",
                    Template = Resources.LicenseApplicationPrimary
                }
            };
        }
    }
}