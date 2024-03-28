namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    ///     Протокол - Реквизиты - В присуствии/отсутствии
    /// </summary>
    public enum TypeRepresentativePresence
    {
        /// <summary>
        ///     в отсутствии
        /// </summary>
        [Display("в отсутствии")]
        None = 0,

        /// <summary>
        ///     в отсутствии
        /// </summary>
        [Display("в присутствии")]
        Yes = 5,

        /// <summary>
        ///     в присутствии законного представителя
        /// </summary>
        [Display("в присутствии законного представителя")]
        LegalRepresentative = 10,

        /// <summary>
        ///     в присутствии уполномоченного представителя
        /// </summary>
        [Display("в присутствии уполномоченного представителя")]
        AuthorizedRepresentative = 20
    }
}