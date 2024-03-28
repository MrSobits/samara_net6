namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Этап нарушения документа "Лист визита"
    /// </summary>
    public class VisitSheetViolation : BaseEntity
    {
        /// <summary>
        /// Информация о группе нарушений
        /// </summary>
        public virtual VisitSheetViolationInfo ViolationInfo { get; set; }

        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual ViolationGji Violation { get; set; }

        /// <summary>
        /// Представляют явную непосредственную угрозу
        /// причинения вреда (ущерба) охраняемым законом ценностям
        /// </summary>
        public virtual bool IsThreatToLegalProtectedValues { get; set; }
    }
}