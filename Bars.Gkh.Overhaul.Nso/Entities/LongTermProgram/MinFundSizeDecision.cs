namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using System;

    /// <summary>
    /// Решение собственников помещений МКД (Минимальный размер фонда КР)
    /// </summary>
    public class MinFundSizeDecision : BasePropertyOwnerDecision
    {
        /// <summary>
        /// Минимальный рамер фонда установленный субъектом
        /// </summary>
        public virtual int SubjectMinFundSize { get; set; }

        /// <summary>
        /// Размер фонда установленный собственниками
        /// </summary>
        public virtual int? OwnerMinFundSize { get; set; }
        
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