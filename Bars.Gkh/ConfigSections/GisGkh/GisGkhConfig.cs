namespace Bars.Gkh.ConfigSections.GisGkh
{

    using Config;
    using Config.Attributes;
    using Config.Attributes.UI;
    using System.ComponentModel;

    /// <summary>
    /// Настройки интеграции с ГИС ЖКХ
    /// </summary>
    [GkhConfigSection("GisGkh", DisplayName = "Интеграция с ГИС ЖКХ")]
    [Permissionable]
    public class GisGkhConfig : IGkhConfigSection
    {
        /// <summary>
        /// Общие
        /// </summary>
        [GkhConfigProperty(DisplayName = "Количество домов для массового запроса информации из ГИС ЖКХ")]
        [DefaultValue(500)]
        public virtual int numberHouseMassExport { get; set; }
    }
}
