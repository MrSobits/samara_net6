namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Основание договора ук с собственниками
    /// </summary>
    public enum ManOrgContractOwnersFoundation
    {
        /// <summary>
        /// Результат открытого конкурса органов местного самоуправления
        /// </summary>
        [Display("Результат открытого конкурса органов местного самоуправления")]
        OpenTenderResult = 10,

        /// <summary>
        /// Протокол общего собрания жильцов
        /// </summary>
        [Display("Протокол общего собрания жильцов")]
        OwnersMeetingProtocol = 20,

        /// <summary>
        /// Договор в соответствии с пунктом 14 статьи 161 ЖКРФ
        /// </summary>
        [Display("Договор в соответствии с пунктом 14 статьи 161 ЖКРФ")]
        InAccordanceArticle161 = 30
    }
}