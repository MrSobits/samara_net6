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

    public class LicenseDuplicateApplicationReport : GkhBaseStimulReport
    {
        private long _requestId;

        private ManOrgLicenseRequest Request { get; set; }

        public IDomainService<ManOrgLicenseRequest> RequestDomain { get; set; }
        public IDomainService<ManOrgLicense> LicenseDomain { get; set; }

        public IDomainService<ManOrgRequestPerson> RequestPersonDomain { get; set; }

        public IDomainService<PersonQualificationCertificate> CertificateDomain { get; set; }

        public IDomainService<PersonPlaceWork> PlaceWorkDomain { get; set; }

        public LicenseDuplicateApplicationReport()
            : base(new ReportTemplateBinary(Resources.LicenseDuplicateApplication))
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

            var licenses = LicenseDomain.GetAll().Where(x => x.Request != null && x.Request.Id == _requestId)
                .Select(x => new
                {
                    x.LicNumber,
                    x.DateRegister
                })
                .ToArray()
                .Select(x =>
                {
                    var result = "№ " + x.LicNumber;
                    if (x.DateRegister.HasValue)
                    {
                        result += " от " + x.DateRegister.Value.ToShortDateString();
                    }

                    return result;
                });
            var licensesString = string.Join(", ", licenses);


            this.ReportParams["СоискательЛицензииСокр"] = Request.Contragent.ShortName;
            this.ReportParams["РеквизитыЛицензии"] = licensesString;
            this.ReportParams["НаименованиеЛицОрганаДатПадеж"] = Request.Contragent.NameDative;
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
        }

        public override string Id
        {
            get { return "LicenseDuplicateApplication"; }
        }

        public override string CodeForm
        {
            get { return "ManOrgLicenseRequest"; }
        }

        public override string Permission
        {
            get { return "Reports.GKH.ManOrgLicenseRequestLicenseDuplicateApplication"; }
        }

        public override string Name
        {
            get { return "Заявление о выдаче дубликата лицензии"; }
        }

        public override string Description
        {
            get { return "Заявление о выдаче дубликата лицензии"; }
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
                    Code = "LicenseDuplicateApplication",
                    Description = "Заявление о выдаче дубликата лицензии",
                    Name = "LicenseDuplicateApplication",
                    Template = Resources.LicenseDuplicateApplication
                }
            };
        }
    }
}