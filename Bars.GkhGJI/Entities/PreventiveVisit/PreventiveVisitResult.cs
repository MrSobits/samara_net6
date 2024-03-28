namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Инспекторы Акта профилактического визита
    /// </summary>
    public class PreventiveVisitResult : BaseEntity
    {
        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual PreventiveVisit PreventiveVisit { get; set; }

        /// <summary>
        /// Проверяемый объект
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Признак выявлено или невыявлено нарушение
        /// </summary>
        public virtual ProfVisitResult ProfVisitResult { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual string InformText { get; set; }
    }
}