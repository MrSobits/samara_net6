namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Да/Нет/Не задано
    /// </summary>
    public enum FuckingOSSState
    {
        [Display("Не задано")]
        Work = 5,

        [Display("Предоставлена копия протокола ОСС")]
        Yes = 10,

        [Display("Отказано в предоставлении копии протокола ОСС")]
        No = 20,

        [Display("Протокол ОСС отсутствует в архиве")]
        NotSet = 30
    }
}
