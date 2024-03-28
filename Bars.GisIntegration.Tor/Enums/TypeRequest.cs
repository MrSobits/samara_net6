namespace Bars.GisIntegration.Tor.Enums
{
    using Bars.B4.Utils;

    public enum TypeRequest
    {
        /// <summary>
        /// Первичное размещение
        /// </summary>
        [Display("Первичное размещение")]
        Initialization,

        /// <summary>
        /// Корректировка
        /// </summary>
        [Display("Корректировка")]
        Correction,

        /// <summary>
        /// Получение
        /// </summary>
        [Display("Получение")]
        Getting
    }
}