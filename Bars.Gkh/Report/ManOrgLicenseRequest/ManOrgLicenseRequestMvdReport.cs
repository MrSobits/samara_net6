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
    /// Запрос в ОМВД
    /// </summary>
    public class ManOrgLicenseRequestMvdReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public ManOrgLicenseRequestMvdReport()
            : base(new ReportTemplateBinary(Properties.Resources.ManOrgLicenseRequestMvdReport))
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
            get { return "Запрос в ОМВД"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Запрос в ОМВД"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "ManOrgLicenseRequestMvdReport"; }
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
            var requestPersonDomain = Container.ResolveDomain<ManOrgRequestPerson>();

            try
            {
                var persons = requestPersonDomain.GetAll()
                    .Where(x => x.LicRequest.Id == _requestId)
                    .ToArray();

                if(persons.IsEmpty()) return;

                var first = persons.First();

                this.ReportParams["ДлФио"] = first.Person.FullName;
                this.ReportParams["ДлДатаРождения"] = first.Person.Birthdate.ToDateString();
            }
            finally
            {
                Container.Release(requestPersonDomain);
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
                    Name = "Запрос в ОМВД",
                    Description = "Запрос в ОМВД",
                    Code = "ManOrgLicenseRequestMvdReport",
                    Template = Properties.Resources.ManOrgLicenseRequestMvdReport
                }
            };
        }

        #endregion Public methods
    }
}