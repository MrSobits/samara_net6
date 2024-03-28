namespace Bars.Gkh.Overhaul.Tat.ConfigSections
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Overhaul.Tat.DomainService;

    /// <summary>
    /// Секция конфигурации "Ограничение на выбор скорректированного года"
    /// </summary>
    public class YearCorrectionConfig : IGkhConfigSection
    {
        /// <summary>
        /// Ограничение на выбор скорректированного года
        /// <para>См. <see cref="IYearCorrectionConfigService"/></para>
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ограничение на выбор скорректированного года")]
        [GkhConfigPropertyEditor("B4.ux.config.YearCorrection", "yearcorrectioneditor")]
        public virtual int Data { get; set; }
    }
}