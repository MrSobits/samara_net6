namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// "В наличии/Отсутствует
    /// </summary>
    public enum TypePresence
    {
        [Display("Не задано")]
        NotSet = 10,
        
        [Display("В наличии")]
        Present = 20,

        [Display("Отсутствует")]
        Absent = 30

    }
}
