namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Гражданство
    /// </summary>
    public enum CitizenshipType
    {
        /// <summary>
        /// Российская федерация
        /// </summary>
        [Display("Российская федерация")]
        RussianFederation = 10,

        /// <summary>
        /// Иное
        /// </summary>
        [Display("Иное")]
        Other = 20
    }
}