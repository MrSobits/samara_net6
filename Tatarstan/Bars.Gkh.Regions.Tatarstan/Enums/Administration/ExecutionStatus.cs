using Bars.B4.Utils;

namespace Bars.Gkh.Regions.Tatarstan.Enums.Administration
{
    /// <summary>
    /// Cтатус выполнения
    /// </summary>
    public enum ExecutionStatus
    {
        /// <summary>
        /// Успешно
        /// </summary>
        [Display("Успешно")]
        Success = 10,

        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        Error = 20,

        /// <summary>
        /// Выполняется
        /// </summary>
        [Display("Выполняется")]
        InProgress = 30
    }
}