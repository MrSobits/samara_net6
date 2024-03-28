namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Периодичность запуска задачи экспорта
    /// </summary>
    public enum TaskPeriodType
    {
        /// <summary>
        /// Однократно
        /// </summary>
        [Display("Однократно")]
        NoPeriodicity,

        /// <summary>
        /// Ежедневно
        /// </summary>
        [Display("Ежедневно")]
        Daily,

        /// <summary>
        /// Еженедельно
        /// </summary>
        [Display("Еженедельно")]
        Weekly,

        /// <summary>
        /// Ежемесячно
        /// </summary>
        [Display("Ежемесячно")]
        Monthly
    }
}