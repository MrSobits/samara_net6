﻿namespace Bars.GkhGji.Report
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
    public class EmailGjiAcceptedReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public EmailGjiAcceptedReport()
            : base(new ReportTemplateBinary(Properties.Resources.EmailGJIReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _emailId;

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
            get { return "Карточка регистрации обращения из электронной почты"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Автогенерируемая при создании обращения из email карточка регистрации электронной почты"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "EmailGjiAccepted"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "EmailGjiAccepted"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Pdf; }
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
                this.ReportParams["Id"] = _emailId;
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
            _emailId = userParamsValues.GetValue<long>("Id");
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
                    Name = "Карточка регистрации обращения из электронной почты",
                    Description = "Автогенерируемая при создании обращения из email карточка регистрации электронной почты",
                    Code = "EmailGjiAccepted",
                    Template = Properties.Resources.EmailGJIAcceptedReport
                }
            };
        }

        #endregion Public methods
    }
}