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
    public class AppealLetterp4st8Report : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public AppealLetterp4st8Report()
            : base(new ReportTemplateBinary(Properties.Resources.AppealLetterp4st8))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _appealCitsAnswerId;

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
            get { return "Направление подвед. ч4 ст8 59ФЗ"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Направление подвед. ч4 ст8 59ФЗ"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "AppealLetterp4st8"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "AppealAnswer1"; }
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
            var appealCitsAnswerDomain = Container.ResolveDomain<AppealCitsAnswer>();

            var data = appealCitsAnswerDomain.Get(_appealCitsAnswerId);

            try
            {
                this.ReportParams["Id"] = data.Id;
            }
            finally
            {
                Container.Release(appealCitsAnswerDomain);
            }

            
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _appealCitsAnswerId = userParamsValues.GetValue<long>("Id");
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
                    Name = "Направление подвед. ч4 ст8 59ФЗ",
                    Description = "О направлении обращения по подведомственности ч4 ст8 59ФЗ",
                    Code = "AppealLetterp4st8",
                    Template = Properties.Resources.AppealLetterp4st8
                }
            };
        }

        #endregion Public methods
    }
}