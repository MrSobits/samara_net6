namespace Bars.GkhGji.Regions.Zabaykalye.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Пункт нарушения в группе (связка нарушения с группой)
    /// </summary>
    public class DocumentViolGroupPoint : BaseEntity
    {
        /// <summary>
        /// Ссылка на группу нарушений
        /// </summary>
        public virtual DocumentViolGroup ViolGroup { get; set; }

        /// <summary>
        /// Ссылка на этап нарушения  
        /// </summary>
        public virtual InspectionGjiViolStage ViolStage { get; set; }
    }
}