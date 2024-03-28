namespace Bars.Gkh.Overhaul.Nso.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Типы решения собственников помещений МКД
    /// </summary>
    public enum PropertyOwnerDecisionType
    {
        [Display("Выбор способа формирования фонда кап.ремонта")]
        SelectMethodForming = 10,

        [Display("Установление минимального размера взноса на кап.ремонт")]
        SetMinAmount = 20,

        [Display("Установление фактических дат проведения КР")]
        ListOverhaulServices = 30,

        [Display("Выбор (смена) владельца специального счета")]
        OwnerSpecialAccount = 40,

        [Display("Выбор кредитной организации")]
        CreditOrganization = 50,

        [Display("Установление минимального размера фонда КР")]
        MinCrFundSize = 60,

        [Display("Установление ранее накопленной суммы на КР (до 01.04.2014)")]
        PreviouslyAccumulatedAmount = 70
    }
}