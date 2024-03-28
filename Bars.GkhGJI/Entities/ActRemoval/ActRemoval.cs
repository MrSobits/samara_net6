namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Enums;

    /// <summary>
    /// Акт проверки устранения нарушений
    /// В данном документе по нарушениям идет устранение и если нарушение устранено
    /// то выставляется дата устранения
    /// </summary>
    public class ActRemoval : DocumentGji
    {
        /// <summary>
        /// Признак устранено или неустранено нарушение
        /// </summary>
        public virtual YesNoNotSet TypeRemoval { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Площадь
        /// </summary>
        public virtual decimal? Area { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        public virtual string Flat { get; set; }
    }
}