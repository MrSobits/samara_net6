namespace Bars.GkhDi.ConfigSections
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Общие
    /// </summary>
    [GkhConfigSection("DiConfig", DisplayName = "Раскрытие информации")]
    [Permissionable]
    public class DiConfig : IGkhConfigSection
    {
        /// <summary>
        /// Расчёт процентов
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расчёт процентов")]
        [Navigation]
        [Permissionable]
        public virtual PercentCalculation PercentCalculation { get; set; }
    }
}
