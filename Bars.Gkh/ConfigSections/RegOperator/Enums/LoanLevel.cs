namespace Bars.Gkh.ConfigSections.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Уровень формирования займов
    /// </summary>
    public enum LoanLevel
    {
        /// <summary>
        /// Регион
        /// </summary>
        [Display("Регион")]
        Region = 0,

        /// <summary>
        /// Муниципальный район
        /// </summary>
        [Display("Муниципальный район")]
        Municipality = 1,

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        [Display("Муниципальное образование")]
        Settlement = 2
    }
}