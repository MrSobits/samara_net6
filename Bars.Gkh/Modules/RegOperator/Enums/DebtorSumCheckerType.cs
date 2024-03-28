namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип определения суммы задолженности
    /// </summary>
    public enum DebtorSumCheckerType
    {
        /// <summary>
        /// Общая сумма
        /// </summary>
        [Display("Общая сумма")]
        Default,

        /// <summary>
        /// По базовому тарифу и тарифу решения
        /// </summary>
        [Display("По базовому тарифу и тарифу решения")]
        BaseAdnDecisionTariff
    }
}