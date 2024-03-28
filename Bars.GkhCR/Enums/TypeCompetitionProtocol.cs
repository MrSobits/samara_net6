namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип протокола конкурса
    /// </summary>
    public enum TypeCompetitionProtocol
    {
        /// <summary>
        /// Протокол вскрытия конвертов
        /// </summary>
        [Display("Протокол вскрытия конвертов")]
        OpenEnvelopes = 10,

        /// <summary>
        /// Протокол рассмотрения заявок
        /// </summary>
        [Display("Протокол рассмотрения заявок")]
        ReviewBids = 20
    }
}