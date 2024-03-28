namespace Bars.Gkh1468.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип  жилого дома
    /// </summary>
    public enum TypeRealObj
    {
        [Display("Без паспорта")]
        NoPassport = 0,

        [Display("Жилой дом")]
        RealityObject = 10,

        [Display("Mногоквартирный")]
        Mkd = 20
    }
}
