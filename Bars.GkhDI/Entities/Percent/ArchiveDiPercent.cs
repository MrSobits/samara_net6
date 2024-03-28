namespace Bars.GkhDi.Entities
{
    /// <summary>
    /// Архив основных процентов раскрытия информации
    /// </summary>
    public class ArchiveDiPercent : BaseArchivePercent
    {
        /// <summary>
        /// Раскрытие информации
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }
    }
}
