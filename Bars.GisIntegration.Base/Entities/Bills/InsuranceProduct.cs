namespace Bars.GisIntegration.Base.Entities.Bills
{
    using System;

    /// <summary>
    /// Страховой продукт
    /// </summary>
    public class InsuranceProduct: BaseRisEntity
    {
        /// <summary>
        /// Наименование вложения
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание вложения
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }

        /// <summary>
        /// Хэш-тег вложения по алгоритму ГОСТ
        /// </summary>
        public virtual string AttachmentHash { get; set; } 

        /// <summary>
        /// Страховая компания
        /// </summary>
        public virtual string InsuranceOrganization { get; set; }

        /// <summary>
        /// Дата закрытия
        /// </summary>
        public virtual DateTime CloseDate { get; set; }
    }
}
