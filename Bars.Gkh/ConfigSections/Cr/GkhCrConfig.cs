namespace Bars.Gkh.ConfigSections.Cr
{
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройки модуля Капитальный ремонт
    /// </summary>
    [GkhConfigSection("GkhCr")]
    [Display("Капитальный ремонт")]
    [Permissionable]
    public class GkhCrConfig : IGkhConfigSection
    {
        /// <summary>
        /// Общие
        /// </summary>
        [GkhConfigProperty]
        [Display("Общие")]
        [Navigation]
        public virtual GeneralConfig General { get; set; }

        /// <summary>
        /// Программа на основе ДПКР
        /// </summary>
        [GkhConfigProperty]
        [Display("Программа на основе ДПКР")]
        [Navigation]
        public virtual DpkrConfig DpkrConfig { get; set; }
    }
}