namespace Bars.Gkh.FormatDataExport.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус удаленной задачи импорта выгружаемых данных
    /// </summary>
    public enum FormadDataExportRemoteStatus
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        Default = 0,

        /// <summary>
        /// В очереди
        /// </summary>
        [Display("В очереди")]
        InQueue = 10,

        /// <summary>
        /// Выполняется
        /// </summary>
        [Display("Выполняется")]
        Running = 20,

        /// <summary>
        /// Успешно
        /// </summary>
        [Display("Успешно")]
        Successfull = 30,

        /// <summary>
        /// Ошибка
        /// </summary>
        [Display("Ошибка")]
        Error = 40,

        /// <summary>
        /// Подготовка
        /// </summary>
        [Display("Подготовка")]
        Pending = 50
    }
}