namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус формирования пакета выгрузки в РИС
    /// </summary>
    public enum SSTUExportState
    {
        /// <summary>
        /// Не выгружено
        /// </summary>
        [Display("Не выгружено")]
        NotExported = 10,

        /// <summary>
        /// Выгружено
        /// </summary>
        [Display("Выгружено")]
        Exported = 20,

        /// <summary>
        /// Ожидает выгрузки
        /// </summary>
        [Display("Ожидает выгрузки")]
        WaitForExport = 30,

        /// <summary>
        /// Выгружено с ошибками
        /// </summary>
        [Display("Выгружено с ошибками")]
        Error = 40,

    }
}