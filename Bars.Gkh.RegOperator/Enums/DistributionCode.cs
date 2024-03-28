namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Код распределения
    /// </summary>
    public enum DistributionCode
    {
        /// <summary>
        /// Ранее накопленные средства
        /// </summary>
        [Display("Ранее накопленные средства")]
        AccumulatedFundsDistribution = 10,

        /// <summary>
        /// Поступление процентов банка
        /// </summary>
        [Display("Поступление процентов банка")]
        BankPercentDistribution = 20,

        /// <summary>
        /// Оплата Стройконтроль
        /// </summary>
        [Display("Оплата Стройконтроль")]
        BuildControlPaymentDistribution = 30,

        /// <summary>
        /// Комиссия за ведение счета кредитной организацией
        /// </summary>
        [Display("Комиссия за ведение счета кредитной организацией")]
        ComissionForAccountServiceDistribution = 40,

        /// <summary>
        /// Субсидия фонда
        /// </summary>
        [Display("Субсидия фонда")]
        FundSubsidyDistribution = 50,

        /// <summary>
        /// Иные источники поступления
        /// </summary>
        [Display("Иные источники поступления")]
        OtherSourcesDistribution = 60,

        /// <summary>
        /// Оплата акта
        /// </summary>
        [Display("Оплата акта")]
        PerformedWorkActsDistribution = 70,

        /// <summary>
        /// Средства за ранее выполненные работы
        /// </summary>
        [Display("Средства за ранее выполненные работы")]
        PreviousWorkPaymentDistribution = 80,

        /// <summary>
        /// Возврат от подрядчика
        /// </summary>
        [Display("Возврат от подрядчика")]
        RefundBuilderDistribution = 90,

        /// <summary>
        /// Возврат взносов на КР
        /// </summary>
        [Display("Возврат взносов на КР")]
        RefundDistribution = 100,

        /// <summary>
        /// Возврат субсидии фонда
        /// </summary>
        [Display("Возврат субсидии фонда")]
        RefundFundSubsidyDistribution = 110,

        /// <summary>
        /// Возврат МСП
        /// </summary>
        [Display("Возврат МСП")]
        RefundMspDistribution = 120,

        /// <summary>
        /// Возврат пени
        /// </summary>
        [Display("Возврат пени")]
        RefundPenaltyDistribution = 130,

        /// <summary>
        /// Возврат региональной субсидии
        /// </summary>
        [Display("Возврат региональной субсидии")]
        RefundRegionalSubsidyDistribution = 140,

        /// <summary>
        /// Возврат стимулирующей субсидии
        /// </summary>
        [Display("Возврат стимулирующей субсидии")]
        RefundStimulateSubsidyDistribution = 150,

        /// <summary>
        /// Возврат целевой субсидии
        /// </summary>
        [Display("Расход по спец счетам регоператора")]
        RefundTargetSubsidyDistribution = 160,

        /// <summary>
        /// Возврат перечисленных средств
        /// </summary>
        [Display("Возврат перечисленных средств")]
        RefundTransferFundsDistribution = 170,

        /// <summary>
        /// Региональная субсидия
        /// </summary>
        [Display("Региональная субсидия")]
        RegionalSubsidyDistribution = 180,

        /// <summary>
        /// Поступление оплат аренды
        /// </summary>
        [Display("Поступление оплат аренды")]
        RentPaymentDistribution = 190,

        /// <summary>
        /// Оплата по мировому соглашению
        /// </summary>
        [Display("Оплата по мировому соглашению")]
        RestructAmicableAgreementDistribution = 200,

        /// <summary>
        /// Стимулирующая субсидия
        /// </summary>
        [Display("Стимулирующая субсидия")]
        StimulateSubsidyDistribution = 210,

        /// <summary>
        /// Целевая субсидия
        /// </summary>
        [Display("Целевая субсидия")]
        TargetSubsidyDistribution = 220,

        /// <summary>
        /// Распределение средств подрядчику
        /// </summary>
        [Display("Распределение средств подрядчику")]
        TransferContractorDistribution = 230,

        /// <summary>
        /// Платеж КР
        /// </summary>
        [Display("Платеж КР.")]
        TransferCrDistribution = 240,

        /// <summary>
        /// Платеж КР
        /// </summary>
        [Display("Платеж КР (РОСП)")]
        TransferCrROSPDistribution = 245,

        /// <summary>
        /// Средства собственников  специального счета
        /// </summary>
        [Display("Средства собственников специального счета")]
        SpecialAccountDistribution = 250,

        /// <summary>
        /// Оплата ПСД
        /// </summary>
        [Display("Оплата ПСД")]
        DEDPaymentDistribution = 260
    }
}