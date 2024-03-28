namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;

    /// <summary>
    /// Настройка зачета средств за ранее выполненные работы
    /// </summary>
    public class PerfWorkChargeConfig : IGkhConfigSection
    {
        /// <summary>
        /// Учет суммы зачета средств за ранее выполненные работы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Учет суммы зачета средств за ранее выполненные работы")]
        [DefaultValue(PerfWorkChargeType.ForExistingCharges)]
        public virtual PerfWorkChargeType PerfWorkChargeType { get; set; }
    }
}