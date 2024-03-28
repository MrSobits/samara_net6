namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    ///     Протокол - Реквизиты - В присуствии/отсутствии
    /// </summary>
    public enum TypeAppelantPresence
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
        [Display("в присутствии представителя")]
        LegalRepresentative = 10,

        /// <summary>
        ///     в присутствии законного представителя
        /// </summary>
        [Display("в отсутствии представителя")]
        NotLegalRepresentative = 15,

        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NetSet = 20,
    }
}