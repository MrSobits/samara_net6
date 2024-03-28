namespace Bars.Gkh.Domain
{
    using System;
    using System.Security.Principal;

    using Bars.B4;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities.Administration.Notification;
    using Bars.Gkh.Enums.Notification;

    /// <summary>
    /// Сервис оповещения пользователей
    /// </summary>
    public interface INotifyService
    {
        /// <summary>
        /// Подключение пользователя
        /// </summary>
        /// <param name="connectionId">Идентификатор подключения <see cref="HubCallerContext.ConnectionId"/></param>
        /// <param name="identity">Идентифицированный пользователь</param>
        void Connect(string connectionId, [NotNull] IIdentity identity);

        /// <summary>
        /// Отключение пользователя
        /// </summary>
        /// <param name="connectionId">Идентификатор подключения <see cref="HubCallerContext.ConnectionId"/></param>
        void Disconnect(string connectionId);

        /// <summary>
        /// Отправить сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        IDataResult SendForAll(NotifyMessage message);

        /// <summary>
        /// Отправить сообщение пользователю
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="user">Получатель</param>
        IDataResult Send(NotifyMessage message, User user);

        /// <summary>
        /// Отправить сообщение всем пользователям роли
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="role">Получатели</param>
        IDataResult Send(NotifyMessage message, Role role);

        /// <summary>
        /// Обработка нажатия кнопки в окне уведомления
        /// </summary>
        /// <param name="connectionId">Идентификатор подключения <see cref="HubCallerContext.ConnectionId"/></param>
        /// <param name="messageId">Идентификатор сообщения <see cref="NotifyMessage.Id"/></param>
        /// <param name="button">Нажатая кнопка</param>
        IDataResult ButtonClick(string connectionId, long messageId, ButtonType button);
    }
}