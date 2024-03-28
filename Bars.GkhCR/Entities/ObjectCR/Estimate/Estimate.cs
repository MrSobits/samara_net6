namespace Bars.GkhCr.Entities
{
    /// <summary>
    /// Смета
    /// </summary>
    public class Estimate : BaseEstimate
    {
        /// <summary>
        /// Сметный расчет по работе
        /// </summary>
        public virtual EstimateCalculation EstimateCalculation { get; set; }
    }
}
