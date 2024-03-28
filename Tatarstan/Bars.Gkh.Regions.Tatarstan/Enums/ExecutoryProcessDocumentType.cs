namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    public enum ExecutoryProcessDocumentType
    {
        /// <summary>
        /// Постановление о возбуждении исполнительного производства
        /// </summary>
        [Display("Постановление о возбуждении исполнительного производства")]
        InitiateExecutoryProcess = 10,

        /// <summary>
        /// Постановление об отложении исполнительного производства
        /// </summary>
        [Display("Постановление об отложении исполнительного производства")]
        DelayExecutoryProcess = 20,

        /// <summary>
        /// Постановление об прекращении исполнительного производства
        /// </summary>
        [Display("Постановление об прекращении исполнительного производства")]
        TerminateExecutoryProcess = 30
    }
}
