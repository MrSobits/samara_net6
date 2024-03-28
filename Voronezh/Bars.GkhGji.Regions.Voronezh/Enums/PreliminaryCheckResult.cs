namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Результат предварительной проверки
    /// </summary>
    public enum PreliminaryCheckResult
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Признаки нарушений не подтвердились
        /// </summary>
        [Display("Признаки нарушений не подтвердились")]
        HasNoViolation = 10,

        /// <summary>
        /// Мотивированное представление
        /// </summary>
        [Display("Мотивированное представление")]
        Motologyc = 20,

    }
}