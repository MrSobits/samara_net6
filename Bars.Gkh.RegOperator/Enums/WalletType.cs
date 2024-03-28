namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Наименование кошелька
    /// </summary>
    public enum WalletType
    {
        /// <summary>
        /// Кошелек оплат по базовому тарифу
        /// </summary>
        [Display("По базовому тарифу")]
        [Description("Кошелек оплат по базовому тарифу")]
        BaseTariffWallet,

        /// <summary>
        /// Кошелек оплат по тарифу решения
        /// </summary>
        [Display("По тарифу решения")]
        [Description("Кошелек оплат по тарифу решения")]
        DecisionTariffWallet,

        /// <summary>
        /// Кошелек оплат по пени
        /// </summary>
        [Display("По пени")]
        [Description("Кошелек оплат по пени")]
        PenaltyWallet,

        /// <summary>
        /// Кошелек оплат по аренде
        /// </summary>
        [Description("Кошелек оплат по аренде")]
        RentWallet,

        /// <summary>
        /// Кошелек оплат по соц поддержке
        /// </summary>
        [Description("Кошелек оплат по соц поддержке")]
        SocialSupportWallet,

        /// <summary>
        /// Кошелек оплат за выполненные работы
        /// </summary>
        [Description("Кошелек оплат за выполненные работы")]
        PreviosWorkPaymentWallet,

        /// <summary>
        /// Кошелек по ранее накопленным средствам
        /// </summary>
        [Description("Кошелек по ранее накопленным средствам")]
        AccumulatedFundWallet,

        /// <summary>
        /// Кошелек оплат по мировому соглашению
        /// </summary>
        [Description("Кошелек оплат по мировому соглашению")]
        RestructAmicableAgreementWallet,

        /// <summary>
        /// Кошелек целевых субсидий
        /// </summary>
        [Description("Кошелек целевых субсидий")]
        TargetSubsidyWallet,

        /// <summary>
        /// Кошелек субсидий фонда
        /// </summary>
        [Description("Кошелек субсидий фонда")]
        FundSubsidyWallet,

        /// <summary>
        /// Кошелек региональных субсидий
        /// </summary>
        [Description("Кошелек региональных субсидий")]
        RegionalSubsidyWallet,

        /// <summary>
        /// Кошелек стимулирующей субсидий
        /// </summary>
        [Description("Кошелек стимулирующей субсидий")]
        StimulateSubsidyWallet,

        /// <summary>
        /// Кошелек иных поступлений
        /// </summary>
        [Description("Кошелек иных поступлений")]
        OtherSourcesWallet,

        /// <summary>
        /// Кошелек процентов банка
        /// </summary>
        [Description("Кошелек процентов банка")]
        BankPercentWallet,

        /// <summary>
        /// Не определен
        /// </summary>
        Unknown = -1
    }
}