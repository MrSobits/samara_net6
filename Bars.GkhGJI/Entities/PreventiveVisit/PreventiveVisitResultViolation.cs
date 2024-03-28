namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Инспекторы Акта профилактического визита
    /// </summary>
    public class PreventiveVisitResultViolation : BaseEntity
    {
        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual PreventiveVisitResult PreventiveVisitResult { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual ViolationGji ViolationGji { get; set; }
    }
}