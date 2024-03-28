using Bars.B4.Utils;

namespace Bars.GkhGji.Enums
{
    /// <summary>
    /// Вид проверки
    /// </summary>
    public enum InspectionKind
    {
        /// <summary>
        /// Плановая проверка
        /// </summary>
        [Display("Плановая проверка")]
        ScheduledInspection = 1,

        /// <summary>
        /// Внеплановая проверка
        /// </summary>
        [Display("Внеплановая проверка")]
        UnscheduledInspection = 2
    }
}
