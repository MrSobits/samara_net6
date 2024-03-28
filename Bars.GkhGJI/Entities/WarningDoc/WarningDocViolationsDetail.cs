namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Связь нарушения требований с нарушением ГЖИ
    /// </summary>
    public class WarningDocViolationsDetail : BaseEntity
    {
        /// <summary>
        /// Нарушение требований
        /// </summary>
        public virtual WarningDocViolations WarningDocViolations { get; set; }

        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual ViolationGji ViolationGji { get; set; }
    }
}