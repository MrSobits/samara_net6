namespace Bars.Gkh.Overhaul.Enum
{
    using B4.Utils;

    public enum CommonParamType
    {
        [Display("Число, целое")]
        Integer = 10,
        [Display("Число, два знака")]
        Decimal = 20,
        [Display("Дата")]
        Date = 30
    }
}