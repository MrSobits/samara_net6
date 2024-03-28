namespace Bars.Gkh.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Entities;

    /// <summary>
    /// Запрос в налоговую
    /// </summary>
    public class ManOrgLicenseRequestTaxReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public ManOrgLicenseRequestTaxReport()
            : base(new ReportTemplateBinary(Properties.Resources.ManOrgLicenseRequestTaxReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _requestId;

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
            get { return "Запрос в налоговую"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Запрос в налоговую"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "ManOrgLicenseTaxRequest"; }
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
            var personDomain = Container.ResolveDomain<ManOrgRequestPerson>();

            try
            {
                var persons = personDomain.GetAll()
                    .Where(x => x.LicRequest.Id == _requestId)
                    .ToArray();

                if(persons.IsEmpty()) return;

                var first = persons.First();

                this.ReportParams["ДлФио"] = first.Person.FullName;
                this.ReportParams["ДлПаспорт"] = string.Format("{0} {1} {2} {3:dd.MM.yyyy}", first.Person.IdSerial, first.Person.IdNumber, first.Person.IdIssuedBy, first.Person.IdIssuedDate);
            }
            finally
            {
                Container.Release(personDomain);
            }
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _requestId = userParamsValues.GetValue<long>("RequestId");
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
                    Code = "ManOrgLicenseRequestTaxReport",
                    Description = "Запрос в налоговую",
                    Name = "Запрос в налоговую",
                    Template = Properties.Resources.ManOrgLicenseRequestTaxReport
                }
            };
        }

        #endregion Public methods
    }
}