namespace Bars.GkhGji.Regions.Tomsk.Report.Licensing
{
    using System.Collections.Generic;
    using B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Gkh.Report;
    using Properties;

    /// <summary>
    /// Шаблон Опись документов
    /// </summary>
    public class ListOfDocumentsReport : GkhBaseStimulReport
    {
    /// <summary>
    /// Шаблон в двоичной системме 
    /// </summary>
        public ListOfDocumentsReport() : base(new ReportTemplateBinary(Resources.ListOfDocuments))
        {
        }
        /// <summary>
        /// Конфигурация поставщика
        /// </summary>
        public IDbConfigProvider ConfigProvider { get; set; }
        /// <summary>
        /// Идентификатор шаблона 
        /// </summary>
        public override string Id
        {
            get { return "ListOfDocuments"; }
        }
        /// <summary>
        /// Код формы 
        /// </summary>
        public override string CodeForm
        {
            get { return "ManOrgLicenseRequest"; }
        }
        /// <summary>
        /// Имя шаблона
        /// </summary>
        public override string Name
        {
            get { return "Опись документов"; }
        }
        /// <summary>
        /// Коментарий
        /// </summary>
        public override string Description
        {
            get { return "Опись документов"; }
        }
        /// <summary>
        /// Код шаблона
        /// </summary>
        protected override string CodeTemplate { get; set; }
        protected long RequestId;
        /// <summary>
        /// Запрос пользователя
        /// </summary>
        /// <param name="userParamsValues"> Пользовательские параметры  </param>
        public override void SetUserParams( UserParamsValues userParamsValues )
        {
            RequestId = userParamsValues.GetValue<long>("RequestId");
        }

        /// <summary> Формат печатной формы </summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }
        /// <summary>
        /// Возрощяет список с название шаблона 
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "ListOfDocuments",
                    Description = "Опись документов",
                    Name = "ListOfDocuments",
                    Template = Resources.ListOfDocuments
                }
            };
        }
        /// <summary>
        /// Строка подключения 
        /// </summary>
        /// <param name="reportParams">Параметры запроса</param>
        public override void PrepareReport( ReportParams reportParams )
        {
            //отчет делается силами внедрения
            this.ReportParams ["ИдентификаторДокументаГЖИ"] = RequestId.ToString();
            this.ReportParams["СтрокаПодключениякБД"] = ConfigProvider.ConnectionString;
            
        }
    }
}
