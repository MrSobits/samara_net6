namespace Bars.Gkh.ConfigSections.RegOperator.Enums.Administration
{
    using Bars.B4.Utils;

    /// <summary>
    /// Уровень логирования при формировании документов на оплату
    /// </summary>
    public enum PaymentDocumentsLogLevel
    {
        /// <summary>
        /// Отключено
        /// </summary>
        [Display("Не формировать лог")]
        None,

        /// <summary>
        /// Только ЛС с ошибками
        /// </summary>
        [Display("Только ЛС с ошибками")]
        Errors = 10,

        /// <summary>
        /// Все ЛС
        /// </summary>
        [Display("Все ЛС")]
        All = 20
    }
}