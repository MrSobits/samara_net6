namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид мероприятия
    /// </summary>
    public enum KindAction
    {
        /// <summary>
        /// Выездное обследование
        /// </summary>
        [Display("Выездное обследование")]
        Survey = 1,

        /// <summary>
        /// Наблюдение за соблюдением обязательных требований.
        /// </summary>
        [Display("Наблюдение за соблюдением обязательных требований")]
        Observation
    }
}
