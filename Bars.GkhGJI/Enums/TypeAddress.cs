namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип адреса
    /// </summary>
    public enum TypeAddress
    {

        [Display("Место регистрации")]
        Place = 0,

        [Display("Место фактического пребывания")]
        PlaceFact = 10,

        [Display("Иное")]
        Other = 20,
    }
}
