namespace Bars.Gkh.RegOperator.Entities.Dict
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Тариф за период для расчета эталонных начислений
    /// </summary>
    public class TariffByPeriodForClaimWork : BaseImportableEntity
    {
        /// <summary>
        /// Наименование 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Наименование 
        /// </summary>
        public virtual ChargePeriod ChargePeriod { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual decimal Value { get; set; }

        /// <summary>
        /// Наименование 
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
