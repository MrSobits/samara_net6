namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Основание подготовка к отопительному сезону
    /// </summary>
    public class BaseHeatSeason : InspectionGji
    {
        /// <summary>
        /// Отопительный сезон
        /// </summary>
        public virtual HeatSeason HeatingSeason { get; set; }
    }
}