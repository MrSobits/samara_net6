namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Таблица для конвертации, связь строго отопительного сезона и схлопнутого
    /// </summary>
    public class HeatSeasExternal : BaseGkhEntity
    {
        /// <summary>
        /// новый идентификатор отопительного сезона
        /// </summary>
        public virtual string NewExternalId { get; set; }
    }
}