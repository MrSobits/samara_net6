namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Профилактическое мероприятие. Способ консультирования
    /// </summary>
    public enum PreventiveActionCounselingType
    {
        /// <summary>
        /// По телефону
        /// </summary>
        [Display("По телефону")]
        Phone = 1,
        
        /// <summary>
        /// Посредством видео-конференц-связи
        /// </summary>
        [Display("Посредством видео-конференц-связи")]
        VideoConferencing = 2
    }
}