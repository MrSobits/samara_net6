namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    public enum LawSuitDebtWorkType
    {
        [Display("Первичное")]
        Mother = 10,

        [Display("Повторное")]
        Father = 20,

        [Display("Третье")]
        Trustee = 30
    }
}