namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Источник распределения
    /// </summary>
    public enum DistributionSource
    {
        /// <summary>
        /// Счет НВС
        /// </summary>
        [Display("Банковская выписка")]
        BankStatement = 10,

        /// <summary>
        /// Банковская выписка
        /// </summary>
        [Display("Счет НВС")]
        SuspenseAccount = 20,

        /// <summary>
        /// Реестр субсидий
        /// </summary>
        [Display("Реестр субсидий")]
        SubsidyIncome = 30
    }
}