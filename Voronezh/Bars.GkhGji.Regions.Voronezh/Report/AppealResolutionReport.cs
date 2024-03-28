namespace Bars.GkhGji.Regions.Voronezh.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Entities;
    using Utils;
    using Gkh.Report;
    using System;
    using B4;

    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Извещение
    /// </summary>
    public class AppealResolutionReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public AppealResolutionReport()
            : base(new ReportTemplateBinary(Properties.Resources.AppealResolutionReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _appealCitsId;

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
            get { return "Резолюция по обращению"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Резолюция по обращению"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "AppealResolutionReport"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "AppealResolutionReport"; }
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
            var appealCitsDomain = Container.ResolveDomain<AppealCits>();

            var data = appealCitsDomain.Get(_appealCitsId);

            try
            {
                this.ReportParams["id"] = Convert.ToInt32(data.Id);
            }
            finally
            {
                Container.Release(appealCitsDomain);
            }

            
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _appealCitsId = userParamsValues.GetValue<long>("Id");
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
                    Name = "Резолюция по обращению",
                    Description = "Печатная форма резолюции по обращению",
                    Code = "AppealResolutionReport",
                    Template = Properties.Resources.AppealResolutionReport
                }
            };
        }

        #endregion Public methods
    }
}