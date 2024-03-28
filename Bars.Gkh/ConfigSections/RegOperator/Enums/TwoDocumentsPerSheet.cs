namespace Bars.Gkh.ConfigSections.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Расположение 2 документов на 1 листе
    /// </summary>
    public enum TwoDocumentsPerSheet
    {
        /// <summary>
        /// По порядку
        /// </summary>
        [Display("По порядку")]
        Ordered,

        /// <summary>
        /// По порядку на половине листа
        /// </summary>
        [Display("По порядку на половине листа")]
        OrderedByHalfSheet
    }
}