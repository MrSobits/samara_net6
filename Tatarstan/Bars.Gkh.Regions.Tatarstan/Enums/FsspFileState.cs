namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статусы загрузки файла
    /// </summary>
    public enum FsspFileState
    {
        /// <summary>
        /// В процессе
        /// </summary>
        [Display("В процессе")]
        InProcess = 10,

        /// <summary>
        /// В очереди
        /// </summary>
        [Display("В очереди")]
        InQueue = 20,

        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        Failed = 30,

        /// <summary>
        /// Загружено с ошибками
        /// </summary>
        [Display("Загружено с ошибками")]
        UploadWithErrors = 31,

        /// <summary>
        /// Успешно
        /// </summary>
        [Display("Успешно")]
        Success = 40
    }
}