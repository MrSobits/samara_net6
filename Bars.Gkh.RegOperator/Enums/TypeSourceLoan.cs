namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Источник займа
    /// </summary>
    public enum TypeSourceLoan
    {
        /// <summary>
        /// Взносы по минимальному тарифу
        /// </summary>
        [Display("Взносы по минимальному тарифу")]
        PaymentByMinTariff = 10,
 
        /// <summary>
        /// Субсидии фонда
        /// </summary>
        [Display("Субсидии фонда")]
        FundSubsidy = 20,

        /// <summary>
        /// Региональная субсидия
        /// </summary>
        [Display("Региональная субсидия")]
        RegionalSubsidy  = 30,

        /// <summary>
        /// Стимулирующая субсидия
        /// </summary>
        [Display("Стимулирующая субсидия")]
        StimulateSubsidy = 40,

        /// <summary>
        /// Целевая субсидия
        /// </summary>
        [Display("Целевая субсидия")]
        TargetSubsidy = 50,

        /// <summary>
        /// Взносы в по тарифу решения
        /// </summary>
        [Display("Взносы по тарифу решения")]
        PaymentByDesicionTariff = 60,

        /// <summary>
        /// Пени
        /// </summary>
        [Display("Пени")]
        Penalty = 70
    }
}
