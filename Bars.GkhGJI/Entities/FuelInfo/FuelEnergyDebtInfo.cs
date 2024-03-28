namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Раздел 4. Задолженность за ранее потребленные топливно-энергетические ресурсы (ТЭР) по состоянию на конец отчетного периода
    /// </summary>
    public class FuelEnergyDebtInfo : BaseFuelInfo
    {
        /// <summary>
        /// Всего
        /// </summary>
        public virtual decimal? Total { get; set; }
    }
}