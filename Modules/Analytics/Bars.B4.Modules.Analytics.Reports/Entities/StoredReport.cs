namespace Bars.B4.Modules.Analytics.Reports.Entities
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;

    /// <summary>
    /// Хранимый отчёт
    /// </summary>
    public class StoredReport : BaseEntity, IReport
    {
        private readonly ICollection<ReportParamGkh> reportParams;

        /// <summary>
        /// Создает новый экземпляр отчета
        /// </summary>
        public StoredReport()
        {
            this.reportParams = new Collection<ReportParamGkh>();
        }

        /// <inheritdoc />
        public virtual string Key => this.Id.ToString();

        /// <summary>
        /// Категория.
        /// </summary>
        public virtual PrintFormCategory Category { get; protected set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// Системное наименование
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Отоброжаемое наименование (может меняться в регионах)
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        /// <summary>
        /// </summary>
        public virtual StoredReportType StoredReportType { get; protected set; }

        /// <summary>
        /// кодировка
        /// </summary>
        public virtual ReportEncoding ReportEncoding { get; protected set; }

        /// <summary>
        /// Файл шаблона
        /// </summary>
        public virtual byte[] TemplateFile { get; protected set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; protected set; }

        /// <summary>
        /// Не хранимое, используется в model.js
        /// </summary>
        public virtual string Roles { get; protected set; }

        /// <summary>
        /// Доступен этот отчет для всех ролей или нет
        /// </summary>
        public virtual bool ForAll { get; set; }

        /// <summary>
        /// Не переопределять определенные в шаблоне строки подключения
        /// </summary>
        public virtual bool UseTemplateConnectionString { get; set; }

        /// <summary>
        /// Генерировать отчёт на сервере расчетов
        /// </summary>
        public virtual bool GenerateOnCalcServer { get; set; }

        /// <summary>
        /// Источник данных
        /// </summary>
        public virtual IList<DataSource> DataSources { get; protected set; }

        /// <summary>
        /// Получить шаблон
        /// </summary>
        /// <returns></returns>
        public virtual Stream GetTemplate()
        {
            var stream = new MemoryStream(this.TemplateFile);
            return stream;
        }

        /// <summary>
        /// Получить источник данных
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IDataSource> GetDataSources()
        {
            return this.DataSources?.ToArray() ?? new IDataSource[] { };
        }

        /// <summary>
        /// Получить параметры
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IParam> GetParams()
        {
            var paramsList = new List<IParam>();
            this.reportParams.ForEach(paramsList.Add);
            paramsList.AddRange(this.DataSources.SelectMany(x => x.Params));
            return paramsList;
        }

        /// <summary>
        /// Получить параметры экспорта
        /// </summary>
        /// <param name="format">Формат</param>
        /// <returns>Настройки отчёта</returns>
        public virtual Dictionary<string, string> GetExportSettings(ReportPrintFormat format)
        {
            if (format == ReportPrintFormat.text)
            {
                var settings = new Dictionary<string, string>
                {
                    { "DrawBorder", "false" },
                    { "PutFeedPageCode", "false" },
                    { "CutLongLines", "false" },
                };

                var encoding = this.GetEncodingForReport();
                if (encoding != null)
                {
                    settings["Encoding"] = encoding;
                }

                return settings;
            }

            if (format == ReportPrintFormat.csv)
            {
                var settings = new Dictionary<string, string>();

                var encoding = this.GetEncodingForReport();
                if (encoding != null)
                {
                    settings["Encoding"] = encoding;
                }

                return settings;
            }

            return null;
        }
        
        /// <summary>
        /// Добавивть файл шаблона
        /// </summary>
        /// <param name="fileData">Файл</param>
        public virtual void AddTemplate(FileData fileData)
        {
            this.TemplateFile = fileData.Data;
        }

        /// <summary>
        /// Установить шаблон
        /// </summary>
        /// <param name="templateBytes">Массив данных</param>
        public virtual void SetTemplate(byte[] templateBytes)
        {
            this.TemplateFile = templateBytes;
        }

        /// <summary>
        /// Добавить параметры
        /// </summary>
        /// <param name="reportParam">Параметры</param>
        public virtual void AddParam(ReportParamGkh reportParam)
        {
            reportParam.AddTo(this);
            this.reportParams.Add(reportParam);
        }

        /// <summary>
        /// Возвращает кодировку в зависимости от текущей настройки для репорта
        /// </summary>
        /// <returns>кодировка либо NULL если не задано</returns>
        private string GetEncodingForReport()
        {
            switch (this.ReportEncoding)
            {
                case ReportEncoding.Utf8:
                    return "utf8";
                case ReportEncoding.Windows1251:
                    return "Windows-1251";
            }

            return null;
        }
    }
}