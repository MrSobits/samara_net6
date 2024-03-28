namespace Bars.GkhDi.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип источника привлеченных средств
    /// </summary>
    public enum TypeSourceFundsDi
    {
        [Display("Субсидии")]
        Subsidy = 10,

        [Display("Кредиты")]
        Credit = 20,

        [Display("Финансирование по договорам лизинга")]
        FinanceByContractLeasing = 30,

        [Display("Финансирование по энергосервисным договорам")]
        FinanceByContractEnergyService = 40,

        [Display("Целевые взносы жителей")]
        PurposeContributionInhabitant = 50,

        [Display("Другие источники")]
        OtherSource = 60,

        [Display("Итого по всем")]
        Summury = 70
    }
}
