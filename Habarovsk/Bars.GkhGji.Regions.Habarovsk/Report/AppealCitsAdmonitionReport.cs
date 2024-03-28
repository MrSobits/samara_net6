namespace Bars.GkhGji.Regions.Habarovsk.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Entities;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Utils;
    using Gkh.Report;
    using System;
    using B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Извещение
    /// </summary>
    public class AppealCitsAdmonitionReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public AppealCitsAdmonitionReport()
            : base(new ReportTemplateBinary(Properties.Resources.AppealCitsAdmonitionReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _appealCitsAdmonitionId;

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
            get { return "Предостережение"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Предостережение"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "AppealCitsAdmonition"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "AppealCitsAdmonition"; }
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
            var appealCitsAdmonitionDomain = Container.ResolveDomain<AppealCitsAdmonition>();

            var data = appealCitsAdmonitionDomain.GetAll()
                .Where(x => x.Id == _appealCitsAdmonitionId).FirstOrDefault();

            try
            {

                this.ReportParams["Id"] = data.Id;

            }
            finally
            {

            }


        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _appealCitsAdmonitionId = userParamsValues.GetValue<long>("Id");
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
                    Name = "Предостережение",
                    Description = "Предостережение",
                    Code = "AppealCitsAdmonitionReport",
                    Template = Properties.Resources.AppealCitsAdmonitionReport
                }
            };
        }

        #endregion Public methods
    }
}