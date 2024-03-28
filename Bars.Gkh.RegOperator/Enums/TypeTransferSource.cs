namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип источника поступления
    /// </summary>
    public enum TypeTransferSource
    {
        /// <summary>
        ///  Не определен
        /// </summary>
        [Display("Не определен")]
        NoDefined = 0,

        /// <summary>
        ///  Банковские выписки
        /// </summary>
        [Display("Банковские выписки")]
        BankAccountStatement = 10,

        /// <summary>
        /// Реестр оплат платежных агентов
        /// </summary>
        [Display("Реестр оплат платежных агентов")]
        BankDocumentImport = 20,

        /// <summary>
        /// Импорт в закрытый период"
        /// </summary>
        [Display("Импорт в закрытый период")]
        ImportsIntoClosedPeriod = 30,

        /// <summary>
        /// Корректировка оплат
        /// </summary>
        [Display("Ручная корректировка оплат")]
        PaymentCorrection = 40,

        /// <summary>
        /// Импорт оплат от ЧЭС
        /// </summary>
        [Display("Импорт оплат от ЧЭС")]
        ImportPayment = 50
    }
}