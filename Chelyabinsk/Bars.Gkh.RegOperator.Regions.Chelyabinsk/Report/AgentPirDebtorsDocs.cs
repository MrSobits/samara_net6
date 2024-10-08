﻿namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Report
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
    public class AgentPirDebtorsDocs : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public AgentPirDebtorsDocs()
            : base(new ReportTemplateBinary(Properties.Resources.AgentPirDebtorsDocs))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _AgentPirId;


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
            get { return "Перечень загруженных документов"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Перечень загруженных документов"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "AgentPirDebtorsDocs"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "AgentPir"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Excel2007; }
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
                this.ReportParams["AGENT_PIR_ID"] = _AgentPirId;
                //Получаем файл с ответом МВД
               

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
            _AgentPirId = userParamsValues.GetValue<long>("Id");
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
                    Name = "Перечень загруженных документов",
                    Description = "Перечень загруженных документов",
                    Code = "AgentPirDebtorsDocs",
                    Template = Properties.Resources.AgentPirLsList
                }
            };
        }    


        #endregion Public methods
    }
}