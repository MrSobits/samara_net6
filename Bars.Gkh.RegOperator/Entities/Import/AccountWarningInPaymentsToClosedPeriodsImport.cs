namespace Bars.Gkh.RegOperator.Entities.Import
{
    /// <summary>
    /// Предупреждение про ЛС при импорте оплаты в закрытый период
    /// </summary>
    public class AccountWarningInPaymentsToClosedPeriodsImport : AccountWarningInClosedPeriodsImport
    {
        /// <summary>
        /// Номер ЛС
        /// </summary>
        public virtual string InnerNumber { get; set; }
        
        /// <summary>
        /// Идентификатор РКЦ
        /// </summary>
        public virtual string InnerRkcId { get; set; }                        
    }
}
