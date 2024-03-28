namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Расположения символов в значении (для настройки формирования номера документа)
    /// </summary>
    public enum SymbolsLocation
    {
        /// <summary>
        /// Все значение
        /// </summary>
        [Display("Все значение")]
        All = 10,

        /// <summary>
        /// В начале
        /// </summary>
        [Display("В начале")]
        Start = 20,

        /// <summary>
        /// В конце
        /// </summary>
        [Display("В конце")]
        End = 30
    }
}