namespace Bars.GkhDi.Entities
{
    /// <summary>
    /// Архив основных процентов по дому
    /// </summary>
    public class ArchiveDiRoPercent : BaseArchivePercent
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual DisclosureInfoRealityObj DiRealityObject { get; set; }
    }
}
