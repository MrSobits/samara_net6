namespace Bars.Gkh.ConfigSections.Administration.FormatDataExport
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Настрока экспорта по формату
    /// </summary>
    [DisplayName("Настройка экспорта данных в РИС ЖКХ")]
    public class FormatDataExportConfig : IGkhConfigSection
    {
        /// <summary>
        /// Максимальная длина поля
        /// </summary>
        public const int MaxWidth = 720;

        /// <summary>
        /// Общие
        /// </summary>
        [GkhConfigProperty(DisplayName = "Общие")]
        public virtual FormatDataExportGeneralConfig FormatDataExportGeneral { get; set; }

        /// <summary>
        /// Параметры экспорта
        /// </summary>
        [GkhConfigProperty(DisplayName = "Параметры экспорта")]
        public virtual FormatDataExportParamsConfig FormatDataExportParams { get; set; }

        /// <summary>
        /// Сопоставление ролей с поставщиками информации
        /// </summary>
        [GkhConfigProperty(DisplayName = "Сопоставление ролей с поставщиками информации")]
        public virtual FormatDataExportRoleConfig FormatDataExportRole { get; set; }

        /// <summary>
        /// Настройки для выгрузки сведений по Региональному оператору
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки для выгрузки сведений по Региональному оператору")]
        public virtual FormatDataExportRegOpConfig FormatDataExportRegOpConfig { get; set; }
    }
}