namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус импорта оплат ЧЭС
    /// </summary>
    public enum ChesImportPaymentsState
    {
        /// <summary>
        /// Не готова к загрузке
        /// </summary>
        [Display("Не готова к загрузке")]
        NotReady = 0,

        /// <summary>
        /// Готова к загрузке
        /// </summary>
        [Display("Готова к загрузке")]
        Ready = 1,
    }
}