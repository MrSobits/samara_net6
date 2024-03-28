namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Отзывы заказчиков о подрядчиках
    /// </summary>
    public class BuilderFeedback : BaseGkhEntity
    {
        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual Builder Builder { get; set; }
        
        /// <summary>
        /// Оценка
        /// </summary>
        public virtual TypeAssessment TypeAssessment { get; set; }

        /// <summary>
        /// Автор
        /// </summary>
        public virtual TypeAuthor TypeAuthor { get; set; }

        /// <summary>
        /// Содержание
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// Название документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Дата отзыва
        /// </summary>
        public virtual DateTime? FeedbackDate { get; set; }

        /// <summary>
        /// Наименование организации
        /// </summary>
        public virtual string OrganizationName { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}
