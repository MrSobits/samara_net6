namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Жилое-нежилое
    /// </summary>
    public enum RiskCategory
    {   
        /// <summary>
        /// Низкий риск
        /// </summary>
        [Display("Не вычисляется")]
        NotSet = 0,
        /// <summary>
        /// Низкий риск
        /// </summary>
        [Display("Низкий риск")]
        Low = 10,

        /// <summary>
        /// Умеренный риск
        /// </summary>
        [Display("Умеренный риск")]
        Moderate = 20,

        /// <summary>
        /// Средний риск
        /// </summary>
        [Display("Средний риск")]
        Average = 30,

        /// <summary>
        /// Высокий риск
        /// </summary>
        [Display("Высокий риск")]
        High = 40,

    }
}