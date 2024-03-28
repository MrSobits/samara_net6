namespace Bars.Gkh.Overhaul.Nso.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус копирования версии программы ДПКР
    /// </summary>
    public enum ProgramVersionCopyingState
    {
        /// <summary>
        /// Не копировалась
        /// </summary>
        [Display("Не копировалась")]
        NotCopied = 0,

        /// <summary>
        /// В процессе
        /// </summary>
        [Display("В процессе")]
        InProcess = 1,

        /// <summary>
        /// Завершено с ошибками
        /// </summary>
        [Display("Завершено с ошибками")]
        CompletedWithError = 2,

        /// <summary>
        /// Завершено успешно
        /// </summary>
        [Display("Завершено успешно")]
        Success = 3
    }
}