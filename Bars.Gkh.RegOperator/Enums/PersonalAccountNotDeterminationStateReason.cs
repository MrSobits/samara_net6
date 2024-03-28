namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    ///  Причина несоответствия ЛС
    /// </summary>
    public enum PersonalAccountNotDeterminationStateReason
    {
        /// <summary>
        /// Не сопоставлен номер ЛС
        /// </summary>
        [Display("Не сопоставлен номер ЛС")]
        AccountNumber = 10,

        /// <summary>
        /// Не сопоставлен номер ЛС из сторонней системы
        /// </summary>
        [Display("Не сопоставлен номер ЛС из сторонней системы")]
        ExternalAccountNumber = 20
    }
}
