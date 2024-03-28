namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    public enum PaymentDocumentFormat
    {
        /// <summary>
        /// В стандартном виде (с ФИО)
        /// </summary>
        [Display("В стандартном виде (с ФИО)")]
        Standard = 10,

        /// <summary>
        /// Обезличено (без ФИО)
        /// </summary>
        [Display("Обезличено (без ФИО)")]
        Depersonalized = 20
    }
}