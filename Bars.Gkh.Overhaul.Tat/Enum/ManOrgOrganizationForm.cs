namespace Bars.Gkh.Overhaul.Tat.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Организационно-правовая форма УО
    /// </summary>
    public enum MoOrganizationForm
    {
        [Display("УК")]
        ManOrg = 10,

        [Display("ТСЖ (более 30 квартир)")]
        TsjOver30Apartments = 20,

        [Display("ТСЖ (1 МКД) или ТСЖ (менее 30 квартир)")]
        TsjLess30Apartments = 30,

        [Display("ЖСК")]
        Jsk = 40,

        [Display("Непосредственное управление")]
        DirectManag = 50
    }
}