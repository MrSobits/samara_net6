namespace Bars.Gkh.Enums
{
    using B4.Utils;

    public enum AttributeType
    {
        [Display("Целое")]
        Int,

        [Display("Вещественное")]
        Decimal,

        [Display("Строка")]
        String,

        [Display("Логическое")]
        Boolean
    }
}