namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Способы уведомления
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// Лично
        /// </summary>
        [Display("Лично")]
        Individually = 1,

        /// <summary>
        /// Нарочно
        /// </summary>
        [Display("Нарочно")]
        Courier = 2,

        /// <summary>
        /// Иное
        /// </summary>
        [Display("Иное")]
        Other = 3,

        /// <summary>
        /// Повестка
        /// </summary>
        [Display("Повестка")]
        Agenda = 4
    }
}