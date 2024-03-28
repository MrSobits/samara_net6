namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System;

    /// <summary>
    /// Решение собственников помещений МКД (Ранее накопленная сумма на КР (до 01.04.2014))
    /// </summary>
    public class PrevAccumulatedAmountDecision : BasePropertyOwnerDecision
    {
        /// <summary>
        /// Накопленнная сумма на КР(до 01.04.2014)
        /// </summary>
        public virtual decimal AccumulatedAmount { get; set; }
        
        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }
    }
}