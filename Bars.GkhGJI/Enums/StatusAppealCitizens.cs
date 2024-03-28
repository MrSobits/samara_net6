namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус обращения граждан (для переноса данных) (коды перенесены один в один, если меняешь коды= менять в Bars.GKH.Converter)
    /// </summary>
    public enum StatusAppealCitizens
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Новое
        /// </summary>
        [Display("Новое")]
        New = 1,

        /// <summary>
        /// В работе
        /// </summary>
        [Display("В работе")]
        InWork = 2,

        /// <summary>
        /// Закрыто
        /// </summary>
        [Display("Закрыто")]
        Closed = 3,

        /// <summary>
        /// Требует отмены
        /// </summary>
        [Display("Требует отмены")]
        CancellationRequired = 4,

        /// <summary>
        /// Отменен
        /// </summary>
        [Display("Отменен")]
        Cancelled = 5
    }
}