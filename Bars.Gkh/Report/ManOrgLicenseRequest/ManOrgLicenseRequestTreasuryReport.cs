namespace Bars.Gkh.Report
{
    using System.Collections.Generic;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Entities;

    /// <summary>
    /// Запрос в федеральное казначейство
    /// </summary>
    public class ManOrgLicenseRequestTreasuryReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public ManOrgLicenseRequestTreasuryReport()
            : base(new ReportTemplateBinary(Properties.Resources.ManOrgLicenseRequestTreasuryReport))
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
            get { return "Запрос в федеральное казначейство"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Запрос в федеральное казначейство"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "ManOrgLicenseRequestTreasuryReport"; }
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
            var requestDomain = Container.ResolveDomain<ManOrgLicenseRequest>();

            try
            {
                var request = requestDomain.Get(_requestId);
                if(request == null) return;

                this.ReportParams["УоИнн"] = request.Contragent.Return(x => x.Inn);
                this.ReportParams["УоНаименование"] = request.Contragent.Return(x => x.Name);
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
                    Name = "Запрос в федеральное казначейство",
                    Description = "Запрос в федеральное казначейство",
                    Code = "ManOrgLicenseRequestTreasuryReport",
                    Template = Properties.Resources.ManOrgLicenseRequestTreasuryReport
                }
            };
        }

        #endregion Public methods
    }
}
