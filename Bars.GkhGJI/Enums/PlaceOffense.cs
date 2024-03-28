namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип адреса
    /// </summary>
    public enum PlaceOffense
    {
        /// <summary>
        /// брать из контрагента юрадрес
        /// </summary>
        [Display("Адрес юрлица")]
        AddressUr = 0,

        [Display("Адрес места возникновения проблемы")]
        PlaceFact = 10,

        /// <summary>
        /// указывается вручную
        /// </summary>
        [Display("Другое")]
        Other = 20,
    }
}
