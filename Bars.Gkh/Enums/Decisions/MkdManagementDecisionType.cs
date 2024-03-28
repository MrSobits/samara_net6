namespace Bars.Gkh.Enums.Decisions
{
    using B4.Utils;

    public enum MkdManagementDecisionType
    {
        [Display("-")]
        NotSet = 0,

        [Display("Непосредственное управление")]
        Direct = 1,

        [Display("ТСЖ (1 МКД) ТСЖ (>1 МКД, <30 кв.)")]
        TsjOneMkd = 2,

        [Display("ТСЖ (>1 МКД)")]
        TsjMoreMkd = 4,

        [Display("Кооператив")]
        Cooperative = 8,

        [Display("Управляющая организация")]
        ManOrg = 16
    }
}