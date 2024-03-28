namespace Bars.Gkh.Enums.Import
{
    using Bars.B4.Utils;

    public enum StatusImport
    {
        /// <summary>
        /// The completed without error.
        /// </summary>
        [Display("Без ошибок")]
        CompletedWithoutError = 10,

        /// <summary>
        /// The completed with warning.
        /// </summary>
        [Display("С предупреждениями")]
        CompletedWithWarning = 20,

        /// <summary>
        /// The completed with error.
        /// </summary>
        [Display("С ошибками")]
        CompletedWithError = 30,

        /// <summary>
        /// Added to task list.
        /// </summary>
        [Display("Добавлена в список задач")]
        AddedTask = 40,
    }
}
