namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    /// <summary>
    /// Результат импорта
    /// </summary>
    public enum ImportResult
    {
        /// <summary>
        /// Не задан
        /// </summary>
        [Display("Не задан")]
        Default = 0,

        /// <summary>
        /// Загружен
        /// </summary>
        [Display("Загружен")]
        Success = 10,

        /// <summary>
        /// Загружен частично
        /// </summary>
        [Display("Загружен частично")]
        Partially = 20,

        /// <summary>
        /// Не загружен
        /// </summary>
        [Display("Не загружен")]
        Error = 30
    }
}