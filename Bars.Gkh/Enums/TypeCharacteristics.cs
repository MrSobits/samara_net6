namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип показателя
    /// </summary>
    public enum TypeCharacteristics
    {
        [Display("Целое")]
        Integer = 10,

        [Display("Дробное")]
        Decimal = 20,

        [Display("Логическое")]
        Boolean = 30,

        [Display("Справочник")]
        Dictionary = 40
    }
}
