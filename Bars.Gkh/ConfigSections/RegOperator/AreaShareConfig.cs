namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройка доли собственности
    /// </summary>
    public class AreaShareConfig : IGkhConfigSection
    {
        /// <summary>
        /// Отображение знаков после запятой в доле собственности
        /// </summary>
        [DefaultValue(2)]
        [UIExtraParam("minValue", 2)]
        [UIExtraParam("maxValue", 7)]
        [GkhConfigProperty(DisplayName = "Отображение знаков после запятой в доле собственности")]
        [GkhConfigPropertyEditor("B4.ux.config.NumberFieldEditor", "numberfieldeditor")]
        public virtual int DecimalsAreaShare { get; set; }
    }
}