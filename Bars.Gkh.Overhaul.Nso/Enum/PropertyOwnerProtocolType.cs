namespace Bars.Gkh.Overhaul.Nso.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип протокола собственников жилых помещений
    /// </summary>
    public enum PropertyOwnerProtocolType
    {
        [Display("Протокол о формировании фонда капитального ремонта")]
        FormationFund = 0,

        [Display("Протокол о выборе управляющей организации")]
        SelectManOrg = 10
    }
}