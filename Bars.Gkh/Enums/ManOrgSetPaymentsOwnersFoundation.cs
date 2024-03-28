namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Основание договора ук с собственниками 
    /// </summary>
    public enum ManOrgSetPaymentsOwnersFoundation
    {
        /// <summary>
        /// Протокол общего собрания жильцов
        /// </summary>
        [Display("Протокол общего собрания жильцов")]
        OwnersMeetingProtocol = 10,

        /// <summary>
        /// Результат открытого конкурса органов местного самоуправления
        /// </summary>
        [Display("Результат открытого конкурса органов местного самоуправления")]
        OpenTenderResult = 20
    }
}