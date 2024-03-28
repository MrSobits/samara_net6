namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Report
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
    using System.Xml.Linq;
    using System.IO;
    using System.Xml;
    using Bars.B4.Modules.FileStorage;
    using System.Security.Cryptography.X509Certificates;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary>
    /// Извещение
    /// </summary>
    public class AgentPirDebtorReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public AgentPirDebtorReport()
            : base(new ReportTemplateBinary(Properties.Resources.AgentPirDebtorReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _DebtorId;


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
            get { return "Расчет задолженности"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Расчет задолженности по ЛС Агента ПИР"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "AgentPirDebtorReport"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "AgentPirDebtor"; }
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
            try
            {
                this.ReportParams["debtId"] = _DebtorId;              

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
            _DebtorId = userParamsValues.GetValue<long>("Id");
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
                    Name = "Расчет задолженности",
                    Description = "Расчет задолженности по ЛС Агента ПИР",
                    Code = "AgentPirDebtorReport",
                    Template = Properties.Resources.AgentPirDebtorReport
                }
            };
        }    


        #endregion Public methods
    }
}