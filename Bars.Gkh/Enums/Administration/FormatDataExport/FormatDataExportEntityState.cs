namespace Bars.Gkh.Enums.Administration.FormatDataExport
{
    using Bars.B4.Utils;
    public enum FormatDataExportEntityState
    {
        /// <summary>
        /// Выполнено успешно
        /// </summary>
        [Display("Выполнено успешно")]
        Success = 1,

        /// <summary>
        /// Не пройдена валидация в РИС.
        /// </summary>
        [Display("Не пройдена валидация в РИС")]
        RisFailure = 2,

        /// <summary>
        /// Выполнено с ошибкой при обработке в ГИС.
        /// </summary>
        [Display("Выполнено с ошибкой при обработке в ГИС")]
        GisFailure = 3
    }
}
