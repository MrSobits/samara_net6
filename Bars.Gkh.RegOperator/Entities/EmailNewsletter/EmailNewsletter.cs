namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using System;

    /// <summary>
    /// Рассылка Email
    /// </summary>
    public class EmailNewsletter : BaseEntity
    {
        /// <summary>
        /// Заголовок
        /// </summary>
        public virtual string Header { get; set; }

        /// <summary>
        /// Содержание
        /// </summary>
        public virtual string Body { get; set; }

        /// <summary>
        /// Адресаты через запятую
        /// </summary>
        public virtual string Destinations { get; set; }

        /// <summary>
        /// Вложение
        /// </summary>
        public virtual FileInfo Attachment { get; set; }

        /// <summary>
        /// Оператор отправителя
        /// </summary>
        public virtual string Sender { get; set; }

        /// <summary>
        /// Успешно?
        /// </summary>
        public virtual bool Success { get; set; }

        /// <summary>
        /// Дата отпрвки
        /// </summary>
        public virtual DateTime? SendDate { get; set; }
    }
}