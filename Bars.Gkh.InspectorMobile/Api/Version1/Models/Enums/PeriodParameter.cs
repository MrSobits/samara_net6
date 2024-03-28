namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Период запроса данных по решениям
    /// </summary>
    public enum PeriodParameter
    {
        /// <summary>
        /// Текущие
        /// </summary>
        [Display("Текущие")]
        Current = 1,
        
        /// <summary>
        /// Предстоящие
        /// </summary>
        [Display("Предстоящие")]
        Upcoming = 2,
        
        /// <summary>
        /// Завершенные
        /// </summary>
        [Display("Завершенные")]
        Completed = 3,
        
        /// <summary>
        /// Вышедшие за период
        /// </summary>
        [Display("Вышедшие за период")]
        OutOfPeriod = 4
    }
}