namespace Bars.GkhDi.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип контрагента
    /// </summary>
    public enum TypeContragentDi
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Собственник")]
        Owner = 20,

        [Display("Арендатор")]
        Renter = 30
    }
}
