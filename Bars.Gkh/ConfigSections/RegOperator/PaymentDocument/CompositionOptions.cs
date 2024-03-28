namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.ComponentModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;

    /// <summary>
    /// Настройки компоновки
    /// </summary>
    public class CompositionOptions : IGkhConfigSection
    {
        /// <summary>
        /// Компоновка квитанций в файле
        /// </summary>
        [GkhConfigProperty(DisplayName = "Компоновка квитанций в файле")]
        [DefaultValue(CompositionType.Account)]
        public virtual CompositionType CompositionType { get; set; }       
    }
}