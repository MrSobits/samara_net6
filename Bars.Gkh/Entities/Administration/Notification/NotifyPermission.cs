namespace Bars.Gkh.Entities.Administration.Notification
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;

    /// <summary>
    /// Права доступа для роли на уведомление
    /// </summary>
    public class NotifyPermission : PersistentObject
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public virtual NotifyMessage Message { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public virtual Role Role { get; set; }
    }
}