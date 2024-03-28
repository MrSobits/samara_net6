namespace Bars.Gkh.Overhaul.Nso.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип владельца специального счета
    /// </summary>
    public enum OwnerAccountDecisionType
    {
        [Display("Региональный оператор")]
        RegOperator = 10,

        [Display("Управляющая компания")]
        UK = 20,

        [Display("ТСЖ")]
        TSJ = 30,

        [Display("ЖСК")]
        JSK = 40,

        [Display("Иной кооператив")]
        OtherCooperative = 50
    }
}