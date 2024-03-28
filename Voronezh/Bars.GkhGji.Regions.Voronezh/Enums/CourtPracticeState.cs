namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус рассмотрения
    /// </summary>
    public enum CourtPracticeState
    {
        /// <summary>
        /// Оставлено без движения
        /// </summary>
        [Display("Оставлено без движения")]
        WithoutMove = 10,

        /// <summary>
        /// Принято к производству
        /// </summary>
        [Display("Принято к производству")]
        ToProduction = 20,

        /// <summary>
        /// Пприостановлено
        /// </summary>
        [Display("Приостановлено")]
        Paused = 30,

        /// <summary>
        /// Завершено
        /// </summary>
        [Display("Завершено")]
        Completed = 40

    }
}