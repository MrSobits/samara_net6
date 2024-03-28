namespace Bars.Gkh.Entities.Administration.Notification
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Enums.Notification;

    /// <summary>
    /// Статистика уведомлений
    /// </summary>
    public class NotifyStats : BaseEntity
    {
        /// <summary>
        /// Нажатая кнопка в окне уведомления
        /// </summary>
        public virtual ButtonType ClickButton { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public virtual NotifyMessage Message { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }
    }
}