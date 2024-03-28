namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип визита профилактического мероприятия
    /// </summary>
    public enum PreventiveActionVisitType
    {
        /// <summary>
        /// Обязательный визит
        /// </summary>
        [Display("Обязательный визит")]
        Required = 1,
        
        /// <summary>
        /// Профилактический визит
        /// </summary>
        [Display("Профилактический визит")]
        Preventive = 2
    }
}