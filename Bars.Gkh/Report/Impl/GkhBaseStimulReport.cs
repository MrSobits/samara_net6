namespace Bars.Gkh.Report
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System;
    using B4.Utils;
    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.StimulReport;

    using Castle.Windsor;

    public abstract class GkhBaseStimulReport : StimulReport, IGkhBaseReport
    {
        public virtual IWindsorContainer Container { get; set; }

        public virtual bool PrintingAllowed
        {
            get
            {
                return true;
            }
        }

        private readonly IReportTemplate reportTemplate;

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public abstract void PrepareReport(ReportParams reportParams);

        protected GkhBaseStimulReport(IReportTemplate reportTemplate)
        {
            this.reportTemplate = reportTemplate;
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public abstract string Id { get; }

        /// <summary>
        /// Права доступа
        /// </summary>
        public virtual string Permission 
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public abstract string CodeForm { get; }

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected abstract string CodeTemplate { get; set; }

        public virtual string Extention
        {
            get { return "mrt"; }
        }

        public virtual string ReportGeneratorName
        {
            get { return "StimulReportGenerator"; }
        }

        /// <summary>
        /// Получить поток шаблона отчета (файла)
        /// </summary>
        /// <returns></returns>
        public virtual Stream GetTemplate()
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

                TemplateInfo template = null;
                if (listTemplatesInfo != null && listTemplatesInfo.Count > 0)
                {
                    template = listTemplatesInfo.FirstOrDefault(x => x.Code == code);
                }

                if (template != null && template.Template != null)
                {
                    return new MemoryStream(template.Template);
                }

                return reportTemplate.GetTemplate();
            }
            catch (FileNotFoundException)
            {
                throw new ReportProviderException("Не удалось получить шаблон замены");
            }
            finally
            {
                Container.Release(templateDomain);
                Container.Release(fileManager);
            }

            
        }

        public virtual string GetFileExtension()
        {
            var templateDomain = Container.Resolve<IDomainService<TemplateReplacement>>();

            try
            {
                string result = null;
                var code = CodeTemplate;

                var listTemplatesInfo = GetTemplateInfo();

                //Если код не был установлен то возвращается первый в списке темплейтов
                if (string.IsNullOrEmpty(code))
                {
                    if (listTemplatesInfo != null && listTemplatesInfo.Count > 0)
                    {
                        code = listTemplatesInfo.FirstOrDefault().Return(x => x.Code);
                    }
                }

                var templateReplace = templateDomain.GetAll().FirstOrDefault(x => x.Code == code);

                if (templateReplace != null && templateReplace.File != null)
                {
                    result = templateReplace.File.Extention.ToLower();
                }

                return "." + result;
            }
            finally
            {
                Container.Release(templateDomain);
            }

        }

        /// <summary>
        /// Получаем имя генератора отчета (DocIOGenerator или XlsIOGenerator)
        /// </summary>
        /// <returns></returns>
        public virtual string GetReportGenerator()
        {
            
            return ReportGeneratorName;
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public abstract void SetUserParams(UserParamsValues userParamsValues);

        public BaseParams BaseParams { get; set; }

        /// <summary>
        /// Получить список шаблонов
        /// </summary>
        /// <returns></returns>
        public abstract List<TemplateInfo> GetTemplateInfo();
    }
}