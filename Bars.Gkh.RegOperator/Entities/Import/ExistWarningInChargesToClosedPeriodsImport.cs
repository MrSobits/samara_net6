namespace Bars.Gkh.RegOperator.Entities.Import
{    
    /// <summary>
    /// Предупреждение про существование начислений при импорте начислений в закрытый период
    /// </summary>
    public class ExistWarningInChargesToClosedPeriodsImport : WarningInClosedPeriodsImport
    {
        /// <summary>
        /// Описатель
        /// </summary>
        public virtual string ChargeDescriptorName { get; set; }
    }
}