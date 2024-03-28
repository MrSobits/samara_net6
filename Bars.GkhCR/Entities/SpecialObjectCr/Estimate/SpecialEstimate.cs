namespace Bars.GkhCr.Entities
{
    /// <summary>
    /// Смета
    /// </summary>
    public class SpecialEstimate : BaseEstimate
    {
        /// <summary>
        /// Сметный расчет по работе
        /// </summary>
        public virtual EstimateCalculation EstimateCalculation { get; set; }
    }
}
