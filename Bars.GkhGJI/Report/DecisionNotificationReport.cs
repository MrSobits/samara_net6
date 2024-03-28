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
    using Bars.GkhGji.Entities;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using System.IO;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    /// <summary>
    /// Извещение
    /// </summary>
    public class DecisionNortificationReport : GkhBaseStimulReport
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public DecisionNortificationReport() : base(new ReportTemplateBinary(Properties.Resources.DecisionNortification))
        {
        }

        private long decisionId;

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get { return "Уведомление о решении"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Уведомление о решении(печатается при условии наличия шаблона замены)"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "DecisionNortification"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "Decision"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Pdf; }
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            try
            {
                this.ReportParams["Id"] = decisionId;
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
            decisionId = userParamsValues.GetValue<long>("DecisionId");
        }

        /// <summary>
        /// Получить поток шаблона отчета (файла)
        /// </summary>
        /// <returns></returns>
        public override Stream GetTemplate()
        {

            var templateDomain = Container.Resolve<IDomainService<TemplateReplacement>>();
            var fileManager = Container.Resolve<IFileManager>();

            try
            {
                var code = CodeTemplate;

                var listTemplatesInfo = GetTemplateInfo();

                if (string.IsNullOrEmpty(code))
                {
                    if (listTemplatesInfo != null && listTemplatesInfo.Count > 0)
                    {
                        code = listTemplatesInfo.FirstOrDefault().Return(x => x.Code);
                    }
                }

                var templateReplace = templateDomain.GetAll().FirstOrDefault(x => x.Code == code);

                if (templateReplace != null)
                {
                    if (fileManager != null)
                    {
                        return fileManager.GetFile(templateReplace.File);
                    }
                }

                throw new Exception("Не назначен шаблон замены");
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Container.Release(templateDomain);
                Container.Release(fileManager);
            }


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
                    Name = "Уведомление о решении",
                    Description = "Уведомление о решении(печатается при условии наличия шаблона замены)",
                    Code = "DecisionNortification",
                    Template = Properties.Resources.DecisionNortification
                }
            };
        }
    }
}