namespace Bars.Gkh.ConfigSections.Overhaul
{
    using Config;
    using Config.Attributes;
    using Config.Attributes.UI;

    /// <summary>
    /// Настройки модуля "Капитальный ремонт"
    /// </summary>
    [GkhConfigSection("Overhaul", DisplayName = "Капитальный ремонт (ДПКР)")]
    [Permissionable]
    public class OverhaulConfig : IGkhConfigSection
    {
    }
}