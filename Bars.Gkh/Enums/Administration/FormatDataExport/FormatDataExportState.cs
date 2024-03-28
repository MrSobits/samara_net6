namespace Bars.Gkh.Enums.Administration.FormatDataExport
{
    using Bars.B4.Utils;
    public enum FormatDataExportState
    {
        /// <summary>
        /// Выполнено успешно
        /// </summary>
        [Display("Выполнено успешно")]
        Success = 1,

        /// <summary>
        /// Выполнено с ошибкой
        /// </summary>
        [Display("Выполнено с ошибкой")]
        Failure = 2
    }
}
