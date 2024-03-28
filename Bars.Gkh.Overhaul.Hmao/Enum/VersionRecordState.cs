using Bars.B4.Utils;

namespace Bars.Gkh.Overhaul.Hmao.Enum
{
    public enum VersionRecordState
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Больше
        /// </summary>
        [Display("Включено в версию")]
        Actual = 10,

        /// <summary>
        /// Равно
        /// </summary>
        [Display("Исключено из версии")]
        NonActual = 20,
    }
}
