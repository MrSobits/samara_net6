namespace Bars.Gkh.Overhaul.Nso.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип организации 
    /// </summary>
    public enum TypeOrganization
    {
        [Display("ТСЖ(менее 30 квартир)")]
        TSJ = 20,

        [Display("ЖСК")]
        JSK = 40,

        [Display("Непосредственное управление")]
        DirectManag = 50,

        [Display("Региональный оператор")]
        RegOperator = 60
    }
}