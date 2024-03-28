namespace Bars.Gkh.ConfigSections.ClaimWork
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    ///     Настройки модуля Претензионная работа
    /// </summary>
    [GkhConfigSection("ClaimWork", DisplayName = "Претензионная работа")]
    [Permissionable]
    public class ClaimWorkConfig : IGkhConfigSection
    {
        /// <summary>
        /// Настройка параметров
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройка параметров")]
        [Navigation]
        public virtual CommonConfig Common { get; set; }

        /// <summary>
        /// Модуль включен
        /// </summary>
        [GkhConfigProperty(Hidden = true)]
        public virtual bool Enabled { get; set; }
    }
}