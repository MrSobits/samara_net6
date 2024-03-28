namespace Bars.GkhCr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип оплаты акта КР (Акт выполненных работ находится в BarsCr)
    /// </summary>
    public enum ActPaymentSrcFinanceType
    {
        [Display("Средства собственников по минимальному тарифу")]
        OwnerFundByMinTarrif = 10,

        [Display("Субсидия фонда")]
        FundSubsidy = 20,

        [Display("Региональная субсидия")]
        RegionSubsidy = 30,

        [Display("Стимулирующая субсидия")]
        StimulSubsidy = 40,

        [Display("Целевая субсидия")]
        TargetSubsidy = 50,

        [Display("Займ")]
        Loan = 60,

        [Display("Средства собственников по тарифу решения")]
        OwnerFundByDecisionTariff = 70,

        [Display("Пени")]
        Penalty = 80,

        [Display("Поступление оплат аренды")]
        Rent = 90,

        [Display("Ранее накопленные средства")]
        AccumulatedFunds = 100,

        [Display("Поступление процентов банка")]
        BankPercent = 110,

        [Display("Иные поступления")]
        Other = 120,

        [Display("Поступление кредита")]
        Credit = 130,

        [Display("Средства за ранее выполненные работы")]
        PreviousWorkPayment = 140,

        [Display("Социальная поддержка")]
        SocialSupport = 150


    }
}