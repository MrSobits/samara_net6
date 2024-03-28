namespace Bars.Gkh.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Entities;
    using Utils;

    /// <summary>
    /// Лицензия УО
    /// </summary>
    public class ManOrgLicenseReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public ManOrgLicenseReport()
            : base(new ReportTemplateBinary(Properties.Resources.ManOrgLicenseReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _licenseId;

        #endregion Private fields

        #region Protected properties

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        #endregion Protected properties

        #region Public properties

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get { return "Лицензия УО"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Лицензия УО"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "ManOrgLicenseReport"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "ManOrgLicense"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat => StiExportFormat.Word2007;
        #endregion Public properties

        #region Public methods

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var licenseDomain = Container.ResolveDomain<ManOrgLicense>();
            var personDomain = Container.ResolveDomain<ManOrgRequestPerson>();

            try
            {
                var license = licenseDomain.Get(_licenseId);

                if(license == null) return;

                this.ReportParams["ЛицензияНомер"] = license.LicNumber;
                this.ReportParams["ЛицензияНомерПриказа"] = license.DisposalNumber;
                this.ReportParams["ЛицензияДатаПриказа"] = license.DateDisposal.ToDateString();

                this.ReportParams["УоНаименование"] = license.Contragent.Return(x => x.Name);
                this.ReportParams["УоКраткоеНаименование"] = license.Contragent.Return(x => x.ShortName);
                this.ReportParams["УоОпф"] = license.Contragent.Return(x => x.OrganizationForm).Return(x => x.Name);
                this.ReportParams["УоОгрн"] = license.Contragent.Return(x => x.Ogrn);
                this.ReportParams["УоИнн"] = license.Contragent.Return(x => x.Inn);

                var persons = personDomain.GetAll()
                    .Where(x => x.LicRequest.Id == license.Request.Id)
                    .ToArray();

                if(persons.IsEmpty()) return;

                var first = persons.First();

                this.ReportParams["ДлФио"] = first.Person.Return(x => x.FullName);
                this.ReportParams["ДлДокТипДокумента"] = first.Person.Return(x => x.TypeIdentityDocument.GetEnumMeta().Display);
                this.ReportParams["ДлДокДатаВыдачи"] = first.Person.Return(x => x.IdIssuedDate.ToDateString());
                this.ReportParams["ДлДокСерия"] = first.Person.Return(x => x.IdSerial);
                this.ReportParams["ДлДокНомер"] = first.Person.Return(x => x.IdNumber);
                this.ReportParams["ДлДокКемВыдан"] = first.Person.Return(x => x.IdIssuedBy);
            }
            finally
            {
                Container.Release(licenseDomain);
                Container.Release(personDomain);
            }
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _licenseId = userParamsValues.GetValue<long>("LicenseId");
        }

        /// <summary>
        /// Получить список шаблонов
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Лицензия УО",
                    Description = "Лицензия УО",
                    Code = "ManOrgLicenseReport",
                    Template = Properties.Resources.ManOrgLicenseReport
                }
            };
        }

        #endregion Public methods
    }
}