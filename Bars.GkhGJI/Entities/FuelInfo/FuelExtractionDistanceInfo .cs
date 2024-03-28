namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Раздел 2. Расстояние от места добычи топлива до потребителя
    /// </summary>
    public class FuelExtractionDistanceInfo : BaseFuelInfo
    {
        /// <summary>
        /// Расстояние
        /// </summary>
        public virtual decimal? Distance { get; set; }
    }
}