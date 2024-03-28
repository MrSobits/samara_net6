namespace Bars.GkhCr.Entities
{
    using B4.DataAccess;
    using Gkh.Modules.ClaimWork.Entities;
    using Modules.ClaimWork.Entities;
    
    /// <summary>
    /// Нарушение в реестре оснований 
    /// </summary>
    public class BuildContractClwViol : BaseEntity
    {
        /// <summary>
        /// Основание ПИР
        /// </summary>
        public virtual BuildContractClaimWork ClaimWork { get; set; }

        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual ViolClaimWork Violation { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Note { get; set; }
    }
}