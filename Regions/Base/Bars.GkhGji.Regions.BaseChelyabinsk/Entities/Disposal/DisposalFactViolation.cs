namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Факты нарушения приказа
    /// </summary>
    public class DisposalFactViolation : BaseEntity
    {
        /// <summary>
        /// Приказ 
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Факты нарушения
        /// </summary>
        public virtual TypeFactViolation TypeFactViolation { get; set; }
    }
}
