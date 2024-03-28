namespace Bars.Gkh.RegOperator.Entities.Import
{    
    /// <summary>
    /// Шапка импорта оплаты в закрытый период
    /// </summary>
    public class HeaderOfPaymentsToClosedPeriodsImport : HeaderOfClosedPeriodsImport
    {        
        /// <summary>
        /// Обновлять сальдо
        /// </summary>
        public virtual bool IsUpdateSaldoIn { get; set; }
        
        /// <summary>
        /// Внешний номер РКЦ
        /// </summary>
        public virtual string ExternalRkcId { get; set; }
    }
}
