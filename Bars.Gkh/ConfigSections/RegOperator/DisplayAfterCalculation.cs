namespace Bars.Gkh.ConfigSections.RegOperator
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Отображение начислений в лицевом счете после расчета
    /// </summary>
    public class DisplayAfterCalculation : IGkhConfigSection
    {
        /// <summary>
        /// Не показывать начисления в лицевом счете
        /// </summary>
        [GkhConfigProperty(DisplayName = "Не показывать начисления в лицевом счете")]
        public virtual bool CalcIsNotActive { get; set; }
    }
}