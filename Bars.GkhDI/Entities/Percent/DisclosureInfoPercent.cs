namespace Bars.GkhDi.Entities
{
    /// <summary>
    /// Процент блока
    /// </summary>
    public class DisclosureInfoPercent : BasePercent
    {
        /// <summary>
        /// Раскрытие информации
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }
    }
}
