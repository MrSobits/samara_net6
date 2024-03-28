namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид выполненного мероприятия действия акта проверки
    /// </summary>
    public enum ActCheckActionCarriedOutEventType
    {
        /// <summary>
        /// Фотосъёмка
        /// </summary>
        [Display("Фотосъёмка")]
        Photography = 1,

        /// <summary>
        /// Видеосъёмка
        /// </summary>
        [Display("Видеосъёмка")]
        Videography = 2,

        /// <summary>
        /// Аудиозапись
        /// </summary>
        [Display("Аудиозапись")]
        AudioRecording = 3
    }
}