namespace Bars.GkhDi.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип категории дома
    /// </summary>
    public enum TypeCategoryHouseDi
    {
        [Display("по домам до 25 лет")]
        To25 = 10,

        [Display("по домам от 26 до 50 лет")]
        From26To50 = 20,

        [Display("по домам от 51 до 75 лет")]
        From51To75 = 30,

        [Display("по домам от 76 лет и более")]
        From76 = 40,

        [Display("по аварийным домам")]
        CrashHouse = 50,

        [Display("Итого по всем")]
        Summury = 60
    }
}
