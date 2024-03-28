namespace Bars.GkhGji.Report
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
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Извещение
    /// </summary>
    public class ResolutionDecisionReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public ResolutionDecisionReport()
            : base(new ReportTemplateBinary(Properties.Resources.AppealCitsDecision))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _entityid;

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
            get { return "Решение по жалобе (постановление)"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Решение по жалобе (постановление)"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "ResolutionDecision"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "ResolutionDecision"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return GetUserFormat(); }
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
                this.ReportParams["Id"] = _entityid;
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
            _entityid = userParamsValues.GetValue<long>("Id");
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
                    Name = "Решение по жалобе (постановление)",
                    Description = "Решение по жалобе",
                    Code = "ResolutionDecision",
                    Template = Properties.Resources.ResolutionDecision
                }
            };
        }

        #endregion Public methods

        private StiExportFormat GetUserFormat()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                var oper = userManager.GetActiveOperator();
                if (oper != null)
                {
                    var ext = oper.ExportFormat;
                    switch (ext)
                    {
                        case OperatorExportFormat.docx:
                            return StiExportFormat.Word2007;
                        case OperatorExportFormat.odt:
                            return StiExportFormat.Odt;
                        case OperatorExportFormat.pdf:
                            return StiExportFormat.Pdf;
                        default: return StiExportFormat.Word2007;
                    }
                }
                else
                {
                    return StiExportFormat.Word2007;
                }

            }
            finally
            {
                this.Container.Release(userManager);
            }

        }
    }
}