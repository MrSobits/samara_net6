namespace Bars.Gkh.Report
{
    using Bars.B4;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using B4.Modules.Reports;
    using System.Linq;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Config;
    using Slepov.Russian.Morpher;
    using Bars.B4.Utils;

    class PersonAgreementDataReport : GkhBaseStimulReport
    {

        #region Fields
        private long personId;

        protected Person Person { get; set; }
        #endregion

        public PersonAgreementDataReport()
            : base(new ReportTemplateBinary(Properties.Resources.PersonAgreementDataReport))
        {
        }

        public IDomainService<Person> PersonDomain { get; set; }

        public IGkhParams GkhParams { get; set; }

        private Склонятель _morpher;

        protected Склонятель GetMorpher()
        {
            return _morpher ?? (_morpher = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ=="));
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            personId = userParamsValues.GetValue<object>("PersonId").ToLong();

            Person = PersonDomain.GetAll().FirstOrDefault(x => x.Id == personId);

            if (Person == null)
            {
                throw new Exception(string.Format("Не удалось определить должностное лицо по Id {0}", personId));
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                    {
                        Code = "PersonRequestToExamReport_1",
                        Name = "Person",
                        Description =
                            "Согласие на обработку персональных данных",
                        Template = Properties.Resources.PersonAgreementDataReport
                    }
            };
        }

        public override string CodeForm
        {
            get { return "Person"; }
        }

        public override string Name
        {
            get { return "Согласие на обработку персональных данных"; }
        }

        public override string Description
        {
            get { return "Должностное лицо - Согласие на обработку персональных данных"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "PersonAgreementDataReport"; }
        }

        public override string Extention
        {
            get { return "mrt"; }
        }

        public override StiExportFormat ExportFormat => StiExportFormat.Word2007;

        public override void PrepareReport(ReportParams reportParams)
        {
            var morpher = GetMorpher();
            var surname = morpher.Проанализировать(Person.Surname ?? "");
            var name = morpher.Проанализировать(Person.Name ?? "");
            var patronymic = morpher.Проанализировать(Person.Patronymic ?? "");

            this.ReportParams["ФИОРодП"] = "{0} {1} {2}".FormatUsing(
                surname != null ? surname.Винительный : "",
                name != null ? name.Винительный : "",
                patronymic != null ? patronymic.Винительный : "");

            this.ReportParams["ДокументСерия"] = Person.IdSerial;
            this.ReportParams["ДокументНомер"] = Person.IdNumber;
            this.ReportParams["ДокументВыдан"] = Person.IdIssuedBy;
            this.ReportParams["ДатаВыдачи"] = Person.IdIssuedDate.HasValue
                ? Person.IdIssuedDate.Value.ToString(CultureInfo.CurrentCulture)
                : DateTime.MinValue.ToString(CultureInfo.CurrentCulture);

            this.ReportParams["ДокументВыданТворП"] = string.Empty;
            if (!string.IsNullOrEmpty(Person.IdIssuedBy))
            {
                var issuedBy = morpher.Проанализировать(Person.IdIssuedBy);

                this.ReportParams["ДокументВыданТворП"] = issuedBy != null ? issuedBy.Творительный : "";
            }

            this.ReportParams["Email"] = Person.Email;
            this.ReportParams["Фамилия"] = Person.Surname;
            this.ReportParams["Имя"] = Person.Name;
            this.ReportParams["Отчество"] = Person.Patronymic;
            this.ReportParams["ТипДокумента"] = Person.TypeIdentityDocument.GetEnumMeta().Display;
            this.ReportParams["АдресРегистрации"] = Person.AddressReg;
            
            var fioSokr = string.Empty;
            
            if( !string.IsNullOrEmpty(Person.Name) && !string.IsNullOrEmpty(Person.Name.Trim()))
            {
                fioSokr += Person.Name.Trim().Substring(0, 1).ToUpper() + ".";
            }

            if (!string.IsNullOrEmpty(Person.Patronymic) && !string.IsNullOrEmpty(Person.Patronymic.Trim()))
            {
                fioSokr += Person.Patronymic.Trim().Substring(0, 1).ToUpper() + ".";
            }

            if (!string.IsNullOrEmpty(Person.Surname) && !string.IsNullOrEmpty(Person.Surname.Trim()))
            {
                fioSokr = string.Format("{0} {1}", fioSokr, Person.Surname.Trim());
            }

            this.ReportParams["ИОФамилия"] = fioSokr.Trim();

        }
    }
}
