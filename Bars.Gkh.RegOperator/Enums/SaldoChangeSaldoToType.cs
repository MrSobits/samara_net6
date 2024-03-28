namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Сальдо, на который переносится при установке/смене сальдо
    /// </summary>
    public enum SaldoChangeSaldoToType
    {
        /// <summary>
        /// На базовой тариф
        /// </summary>
        [Display("На базовой тариф")]
        BaseTariff = 10,

        /// <summary>
        /// На тариф решения
        /// </summary>
        [Display("На тариф решения")]
        DecisionTariff = 20,

        /// <summary>
        /// На пени
        /// </summary>
        [Display("На пени")]
        Penalty = 30
    }
}