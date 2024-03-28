namespace Bars.GkhGji.Regions.Tula.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Сущность для хранения больших объекмов текста по Группе нарушения
    /// </summary>
    public class DocumentViolGroupLongText : BaseEntity
    {
        /// <summary>
        /// Ссылка на группу нарушений
        /// </summary>
        public virtual DocumentViolGroup ViolGroup { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual byte[] Description { get; set; }

        /// <summary>
        /// Мероприятие по устранению
        /// </summary>
        public virtual byte[] Action { get; set; }
    }
}