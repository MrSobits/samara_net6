namespace Bars.GkhGji.Regions.Zabaykalye.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Группа нарушений для Дкумента ГЖИ
    /// Здесь будет вестись информация о описаниях, мероприятиях, 
    /// датах начал, датах окончания по группе нарушений
    /// </summary>
    public class DocumentViolGroup : BaseEntity
    {
        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Ссылка на документ гжи
        /// </summary>
        public virtual DocumentGji Document { get; set; }

        /// <summary>
        /// Краткое описание (Полное поисание хранится в сущности DocumentViolGroupLongText)
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Краткое мероприятие по устронению (Полное мероприятие хранится в сущности DocumentViolGroupLongText)
        /// </summary>
        public virtual string Action { get; set; }

        /// <summary>
        /// Плановая дата устранения
        /// </summary>
        public virtual DateTime? DatePlanRemoval { get; set; }

        /// <summary>
        /// Фактическая дата устранения
        /// </summary>
        public virtual DateTime? DateFactRemoval { get; set; }
    }
}