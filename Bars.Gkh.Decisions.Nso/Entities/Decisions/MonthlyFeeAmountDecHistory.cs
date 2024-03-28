namespace Bars.Gkh.Decisions.Nso.Entities
{
    using System.Collections.Generic;
    
    using Bars.Gkh.Entities;

    /// <summary>
    ///     Размер ежемесячного взноса на КР
    /// </summary>
    public class MonthlyFeeAmountDecHistory : BaseImportableEntity
    {
        /// <summary>
        ///     Помесячные взносы
        /// </summary>
        public virtual List<PeriodMonthlyFee> Decision { get; set; }

        /// <summary>
        ///     Ответственный
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        ///     Решения собственников жилья
        /// </summary>
        public virtual RealityObjectDecisionProtocol Protocol { get; set; }
    }
}