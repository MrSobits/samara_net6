namespace Bars.Gkh.Gis.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип результата сопоставления адреса
    /// </summary>
    public enum TypeAddressMatched
    {
        [Display("Адрес не сопоставлялся")]
        NotMatched = 10,

        [Display("Адрес сопоставлен, но не найден")]
        MatchedNotFound = 20,

        [Display("Адрес успешно сопоставлен. Дом не найден в ЖФ")]
        MatchedNotFoundRealityObject = 30,

        [Display("Адрес успешно сопоставлен")]
        MatchedFound = 40
    } 
}