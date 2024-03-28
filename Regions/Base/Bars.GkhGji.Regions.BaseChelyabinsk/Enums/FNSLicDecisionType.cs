namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид КНД
    /// </summary>
    public enum FNSLicDecisionType
    {
        /// <summary>
        /// Предоставление лицензии
        /// </summary>
        [Display("Предоставление лицензии")]
        MakeLicense = 1,

        /// <summary>
        /// переоформление (продление срока действия)
        /// </summary>
        [Display("Переоформление")]
        Reissuance = 2,

        /// <summary>
        /// Приостановление действия лицензии
        /// </summary>
        [Display("Приостановление действия лицензии")]
        Pause = 3,

        /// <summary>
        /// Возобновление действия лицензии
        /// </summary>
        [Display("Возобновление действия лицензии")]
        Restart = 4,

        /// <summary>
        /// Аннулирование лицензии
        /// </summary>
        [Display("Аннулирование лицензии")]
        Cancellation = 5,

        /// <summary>
        /// Признание лицензии утратившей силу
        /// </summary>
        [Display("Признание лицензии утратившей силу")]
        Invalid = 6,

        /// <summary>
        /// Ограничение действия лицензии (снятие ограничения)
        /// </summary>
        [Display("Ограничение действия лицензии (снятие ограничения)")]
        Limitation = 7,

        /// <summary>
        /// Отзыв лицензии
        /// </summary>
        [Display("Отзыв лицензии")]
        Revocation = 8,

    }
}