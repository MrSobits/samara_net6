namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус импорта ЧЭС
    /// </summary>
    public enum ChesImportState
    {
        /// <summary>
        /// Загружается
        /// </summary>
        [Display("Загружается")]
        Loading = 10,

        /// <summary>
        /// Загружен
        /// </summary>
        [Display("Загружен")]
        Loaded = 20,

        /// <summary>
        /// Загружен с ошибками
        /// </summary>
        [Display("Загружен с ошибками")]
        LoadedWithError = 30
    }
}