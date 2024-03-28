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
    public class ManOrgRequestLicComissionReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public ManOrgRequestLicComissionReport()
            : base(new ReportTemplateBinary(Properties.Resources.ManOrgRequestLicComissionReport))
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
            get { return "Заключение лицензионной комиссии"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Заключение лицензионной комиссии"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "ManOrgRequestLicComissionReport"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "ManOrgRequestReport"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
            set { }
        }

        #endregion Public properties

        #region Public methods

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var requestDomain = Container.ResolveDomain<ManOrgLicenseRequest>();

            try
            {
                var request = requestDomain.Get(_requestId);

               

                this.ReportParams["id"] = request.Id;
            }
            finally
            {
                Container.Release(requestDomain);
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
                    Name = "Заключение лицензионной комиссии",
                    Description = "Заключение лицензионной комиссии",
                    Code = "ManOrgRequestLicComissionReport",
                    Template = Properties.Resources.ManOrgRequestLicComissionReport
                }
            };
        }

        #endregion Public methods
    }
}