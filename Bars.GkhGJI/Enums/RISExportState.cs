namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус формирования пакета выгрузки в РИС
    /// </summary>
    public enum RISExportState
    {
        /// <summary>
        /// Пакет не сформирован
        /// </summary>
        [Display("Пакет не сформирован")]
        NoSet = 10,

        /// <summary>
        /// Формирование пакета
        /// </summary>
        [Display("Формирование пакета")]
        Forming = 20,

        /// <summary>
        /// Пакет сформирован
        /// </summary>
        [Display("Пакет сформирован")]
        Formed = 30,

        /// <summary>
        /// Пакет сформирован
        /// </summary>
        [Display("Ошибка формирования пакета")]
        Error = 40
    }
}