namespace Bars.Gkh.ConfigSections.Administration
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.ConfigSections.Administration.FormatDataExport;
    using Bars.Gkh.ConfigSections.Administration.Import;
    using Bars.Gkh.ConfigSections.Administration.PortalExport;
    using Bars.Gkh.ConfigSections.RegOperator.Administration;

    /// <summary>
    /// Администрирование
    /// </summary>
    [GkhConfigSection("Administration")]
    [Permissionable]
    [DisplayName(@"Администрирование")]
    public class AdministrationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Импорт
        /// </summary>
        [GkhConfigProperty]
        [DisplayName(@"Импорт")]
        [Navigation]
        public virtual ImportConfig Import { get; set; }

        /// <summary>
        /// Логи
        /// </summary>
        [GkhConfigProperty]
        [DisplayName(@"Логи")]
        [Navigation]
        public virtual LogsConfig Logs { get; set; }

        /// <summary>
        /// Настройка экспорта данных в РИС ЖКХ
        /// </summary>
        [GkhConfigProperty]
        [DisplayName(@"Настройка экспорта данных в РИС ЖКХ")]
        [Navigation]
        public virtual FormatDataExportConfig FormatDataExport { get; set; }
        
        /// <summary>
        /// Настройка экспорта данных на портал
        /// </summary>
        [GkhConfigProperty]
        [DisplayName(@"Настройка экспорта данных на портал")]
        [Navigation]
        public virtual PortalExportConfig PortalExportConfig { get; set; }

        /// <summary>
        /// Настройка сложности пароля
        /// </summary>
        [GkhConfigProperty]
        [DisplayName(@"Настройка сложности пароля")]
        [Navigation]
        public virtual UserPasswordConfig UserPasswordConfig { get; set; }
    }
}