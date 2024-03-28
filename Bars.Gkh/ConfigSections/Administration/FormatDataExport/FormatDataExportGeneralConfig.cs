namespace Bars.Gkh.ConfigSections.Administration.FormatDataExport
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Общие
    /// </summary>
    [DisplayName("Общие")]
    public class FormatDataExportGeneralConfig : IGkhConfigSection
    {
        /// <summary>
        /// Запускать экспорт на сервере расчетов
        /// </summary>
        [GkhConfigProperty(DisplayName = "Запускать экспорт на сервере расчетов")]
        [DefaultValue(true)]
        public virtual bool StartInExecutor { get; set; }

        /// <summary>
        /// Адрес удаленного сервера для передачи данных
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес удаленного сервера для передачи данных")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual string TransferServiceAddress { get; set; }

        /// <summary>
        /// Токен авторизации удаленного сервера для передачи данных
        /// </summary>
        [GkhConfigProperty(DisplayName = "Токен авторизации удаленного сервера для передачи данных")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual string TransferServiceToken { get; set; }

        /// <summary>
        /// Часовая зона
        /// </summary>
        [GkhConfigProperty(DisplayName = "Часовая зона")]
        [DefaultValue(TimeZoneType.EuropeMoscow)]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual TimeZoneType TimeZone { get; set; }

        /// <summary>
        /// Время ожидания загрузки файла (в минутах)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Время ожидания загрузки файла (в минутах)")]
        [DefaultValue(10)]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual int Timeout { get; set; }
    }
}