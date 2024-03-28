namespace Bars.Gkh.Report
{
    using System.Collections.Generic;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using Entities;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary>
    /// Извещение
    /// </summary>
    public class CSCalculationReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public CSCalculationReport()
            : base(new ReportTemplateBinary(Properties.Resources.CSCalculation))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _CSCalculationId;

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
            get { return "Протокол расчета"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Печатная форма по расчету платы ЖКУ в модуле Электронный инспектор"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "AppealAnswer1"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "CSCalculation"; }
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
            var csCalculationDomain = Container.ResolveDomain<CSCalculation>();

            var data = csCalculationDomain.Get(_CSCalculationId);

            try
            {
                this.ReportParams["Id"] = data.Id;
                this.ReportParams["FormulaName"] = data.Id;
            }
            finally
            {
                Container.Release(csCalculationDomain);
            }

            
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _CSCalculationId = userParamsValues.GetValue<long>("Id");
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
                    Name = "Протокол расчета",
                    Description = "Печатная форма по расчету платы ЖКУ в модуле Электронный инспектор",
                    Code = "CSCalculation",
                    Template = Properties.Resources.CSCalculation
                }
            };
        }

        #endregion Public methods
    }
}