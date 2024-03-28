namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип документов претензионно-исковой работы
    /// </summary>
    public enum ClaimWorkDocumentType
    {
        /// <summary>
        /// Уведомление
        /// </summary>
        [Display("Уведомление")]
        Notification = 10,

        /// <summary>
        /// Претензия
        /// </summary>
        [Display("Претензия")]
        Pretension = 20,

        /// <summary>
        /// Исковое заявление
        /// </summary>
        [Display("Исковое заявление")]
        Lawsuit = 30,

        /// <summary>
        /// Акт выявления нарушения
        /// </summary>
        [Display("Акт выявления нарушения")]
        ActViolIdentification = 40,

        /// <summary>
        /// Реструктуризация долга
        /// </summary>
        [Display("Реструктуризация долга")]
        RestructDebt = 50,

        /// <summary>
        /// Реструктуризация по мировому соглашению
        /// </summary>
        [Display("Реструктуризация по мировому соглашению")]
        RestructDebtAmicAgr = 51,

        /// <summary>
        /// Заявление о выдаче судебного приказа
        /// </summary>
        [Display("Заявление о выдаче судебного приказа")]
        CourtOrderClaim = 60,

        /// <summary>
        /// Заявление об отказе от исковых требований
        /// </summary>
        [Display("Заявление об отказе от исковых требований")]
        RefusalLawsuit = 70,

        /// <summary>
        /// Исполнительное производство
        /// </summary>
        [Display("Исполнительное производство")]
        ExecutoryProcess = 80,

        /// <summary>
        /// Постановление о наложении ареста на имущество
        /// </summary>
        [Display("Постановление о наложении ареста на имущество")]
        SeizureOfProperty = 90,

        /// <summary>
        /// Постановление об ограничении выезда из РФ 
        /// </summary>
        [Display("Постановление об ограничении выезда из РФ")]
        DepartureRestriction = 100
    }
}