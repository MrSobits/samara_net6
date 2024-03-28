namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Нарушения в постановлении Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorViolation : BaseGkhEntity
    {
        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual ActCheckViolation Violation { get; set; }

        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public virtual ResolutionRospotrebnadzor Resolution { get; set; }
    }
}