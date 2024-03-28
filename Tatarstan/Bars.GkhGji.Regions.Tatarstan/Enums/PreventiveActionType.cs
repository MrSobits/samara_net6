namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид профилактического мероприятия
    /// </summary>
    public enum PreventiveActionType
    {
        /// <summary>
        /// Консультация
        /// </summary>
        [Display("Консультация")]
        Consultation = 1,
        
        /// <summary>
        /// Визит
        /// </summary>
        [Display("Визит")]
        Visit = 2
    }
}