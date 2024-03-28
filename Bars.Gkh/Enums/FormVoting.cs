namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Форма проведения голосования
    /// </summary>
    public enum FormVoting
    {
        /// <summary>
        /// Заочное голосование
        /// </summary>
        [Display("Заочное голосование")]
        Extramural = 1,

        /// <summary>
        /// Очное голосование
        /// </summary>
        [Display("Очное голосование")]
        Intramural = 2,

        /// <summary>
        /// Заочное голосование с использованием системы
        /// </summary>
        [Display("Заочное голосование с использованием системы")]
        ExtramuralUsingSystem = 3,

        /// <summary>
        /// Очно-заочное голосование
        /// </summary>
        [Display("Очно-заочное голосование")]
        FullTime = 4
    }
}