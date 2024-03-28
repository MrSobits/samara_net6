namespace Bars.Gkh.Entities.Administration.Notification
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Enums.Notification;

    using Newtonsoft.Json;

    /// <summary>
    /// Сообщение-уведомление
    /// </summary>
    public class NotifyMessage : BaseEntity
    {
        /// <summary>
        /// Удалено
        /// </summary>
        public virtual bool IsDelete { get; set; }

        /// <summary>
        /// Актуально с
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Актуально по
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// Заголовок сообщения
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// Набор кнопок
        /// </summary>
        public virtual ButtonType ButtonSet { get; set; }

        /// <summary>
        /// Отправитель
        /// </summary>
        [JsonIgnore]
        public virtual User User { get; set; }
    }
}