namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Сальдо, с которого переносится при установке/смене сальдо
    /// </summary>
    public enum SaldoChangeSaldoFromType
    {
        /// <summary>
        /// С базового тарифа
        /// </summary>
        [Display("С базового тарифа")]
        BaseTariff = 10,

        /// <summary>
        /// С тарифа решения
        /// </summary>
        [Display("С тарифа решения")]
        DecisionTariff = 20,

        /// <summary>
        /// С пени
        /// </summary>
        [Display("С пени")]
        Penalty = 30
    }
}