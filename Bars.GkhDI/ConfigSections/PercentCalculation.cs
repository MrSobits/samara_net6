namespace Bars.GkhDi.ConfigSections
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Секция расчёта процентов
    /// </summary>
    public class PercentCalculation : IGkhConfigSection
    {
        /// <summary>
        /// Производить расчет процентов раскрытия информации согласно 988 ПП РФ на сервере расчётов
        /// </summary>
        [GkhConfigProperty(DisplayName = "Производить расчет процентов на сервере расчётов")]
        public virtual bool DiCalcOnExecutor { get; set; }

        /// <summary>
        /// Количество одновременного количества расчетов процента заполнения согласно 988 ПП РФ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Количество одновременного количества расчетов процента заполнения согласно 988 ПП РФ")]
        [DefaultValue(10)]
        public virtual int IsCalculationCount { get; set; }
    }
}