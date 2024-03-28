using Bars.B4.Utils;

namespace Bars.GkhGji.Enums
{
    /// <summary>
    /// Статус оплаты постановления (совместимо с YesNoNotSet)
    /// </summary>
    public enum ResolutionPaymentStatus
    {
        /// <summary>
        /// Оплачен
        /// </summary>
        [Display("Оплачен")]
        Paid = 10,

        /// <summary>
        /// Не оплачен
        /// </summary>
        [Display("Не оплачен")]
        NotPaid = 20,

        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 30,

        /// <summary>
        /// Частично оплачен
        /// </summary>
        [Display("Частично оплачен")]
        PartialPaid = 40,

        /// <summary>
        /// Переплата
        /// </summary>
        [Display("Переплата")]
        OverPaid = 50
    }
}