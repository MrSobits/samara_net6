using Bars.B4.DataAccess;
using Bars.Gkh.Modules.ClaimWork.Entities;

namespace Bars.GkhCr.Entities
{

    /// <summary>
    /// Нарушение в реестре подрядчиков 
    /// </summary>
    public class BuilderViolatorViol : BaseEntity
    {
        /// <summary>
        /// Нарушитель договоров
        /// </summary>
        public virtual BuilderViolator BuilderViolator { get; set; }

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