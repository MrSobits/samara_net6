namespace Bars.Gkh.RegOperator.Entities.Import
{
    /// <summary>
    /// Шапка импорта начислений в закрытый период
    /// </summary>
    public class HeaderOfChargesToClosedPeriodsImport : HeaderOfClosedPeriodsImport
    {     
        /// <summary>
        /// Не обновлять сальдо
        /// </summary>
        public virtual bool WithoutSaldo { get; set; }     
    }
}