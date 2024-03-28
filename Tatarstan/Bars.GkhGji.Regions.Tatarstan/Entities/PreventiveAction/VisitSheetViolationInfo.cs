namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Информация о группе нарушений документа "Лист визита"
    /// </summary>
    public class VisitSheetViolationInfo : BaseEntity
    {
        /// <summary>
        /// Лист визита
        /// </summary>
        public virtual VisitSheet VisitSheet { get; set; }

        /// <summary>
        /// Дом нарушения (Место выявления)
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}