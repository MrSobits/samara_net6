namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид права в отношении объекта
    /// </summary>
    public enum KindRightToObject
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Общая долевая собственность")]
        TotalShareOwnership = 10,

        [Display("Совместная собственность")]
        JoinOwnership = 20,

        [Display("Собственность")]
        Ownership = 30
    }
}
