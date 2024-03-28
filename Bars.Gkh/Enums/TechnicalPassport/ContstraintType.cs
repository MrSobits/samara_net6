namespace Bars.Gkh.Enums.TechnicalPassport
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип ограничения
    /// </summary>
    public enum ContstraintType
    {
        /// <summary>
        /// Минимальное значение
        /// </summary>
        [Display("Минимальное значение")]
        MinValue = 10,

        /// <summary>
        /// Максимальное значение
        /// </summary>
        [Display("Максимальное значение")]
        MaxValue = 20,

        /// <summary>
        /// Минимальная длина
        /// </summary>
        [Display("Минимальная длина")]
        MinLength = 30,

        /// <summary>
        ///  Максимальная длина
        /// </summary>
        [Display("Максимальная длина")]
        MaxLength = 40,

        /// <summary>
        /// Количество знаков после запятой
        /// </summary>
        [Display("Количество знаков после запятой")]
        Decimals = 50
    }
}
