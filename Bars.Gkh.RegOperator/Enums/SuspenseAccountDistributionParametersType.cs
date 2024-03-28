namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// ТИП Для параметров распределения 
    /// </summary>
    public enum SuspenseAccountDistributionParametersType
    {
        /// <summary>
        /// Субсидия фонда
        /// </summary>
        [Display("Субсидия фонда")]
        SubsidyFund = 10,

        /// <summary>
        /// Региональная субсидия
        /// </summary>
        [Display("Региональная субсидия")]
        RegionalSubsidy = 20,

        /// <summary>
        /// Стимулирующая субсидия
        /// </summary>
        [Display("Стимулирующая субсидия")]
        StimulatingSubsidy = 30,

        /// <summary>
        /// Целевая субсидия
        /// </summary>
        [Display("Целевая субсидия")]
        GrantInAid = 40,

        /// <summary>
        /// Платеж КР
        /// </summary>
        [Display("Платеж КР")]
        TransferCr = 50,

        /// <summary>
        /// Платеж КР
        /// </summary>
        [Display("Платеж КР (РОСП)")]
        TransferCrROSP = 55,

        /// <summary>
        /// Оплата акта
        /// </summary>
        [Display("Оплата акта")]
        ActPayment = 60,

        /// <summary>
        /// Бюджет муниципального образования
        /// </summary>
        [Display("Бюджет МО")]
        BudgetMu = 70,

        /// <summary>
        /// Бюджет субъекта
        /// </summary>
        [Display("Бюджет субъекта")]
        BudgetSubject = 80,

        /// <summary>
        /// Средства фонда
        /// </summary>
        [Display("Средства фонда")]
        FundResource = 90,

        /// <summary>
        /// Поступление оплат аренды
        /// </summary>
        [Display("Поступление оплат аренды")]
        RentPaymentIn = 100,

        /// <summary>
        /// Ранее накопленные средства
        /// </summary>
        [Display("Ранее накопленные средства")]
        AccumulatedFunds = 110,

        /// <summary>
        /// Иные поступления
        /// </summary>
        [Display("Иные поступления")]
        OtherSources = 120,

        /// <summary>
        /// Поступление процентов банка
        /// </summary>
        [Display("Поступление процентов банка")]
        BankPercent = 130,

        /// <summary>
        /// Средства за ранее выполненные работы
        /// </summary>
        [Display("Средства за ранее выполненные работы")]
        PreviousWorkPayments = 140,

        /// <summary>
        /// Погашение кредита
        /// </summary>
        [Display("Погашение кредита")]
        CreditRepayment = 150,

        /// <summary>
        /// Погашение процентов по кредиту
        /// </summary>
        [Display("Погашение процентов по кредиту")]
        CreditPercentRepayment = 160,

        /// <summary>
        /// Расход на получение гарантий и поручительства по кредитам
        /// </summary>
        [Display("Расход на получение гарантий и поручительства по кредитам")]
        CreditGuarantee = 170
    }
}