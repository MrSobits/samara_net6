namespace Bars.Gkh.SignalR
{
    using System;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums.Notification;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// Хаб для работы с уведомлениями пользователей
    /// </summary>
    [Authorize]
    public class NotifyHub : Hub<INotifyHubClient>
    {
        private readonly Lazy<INotifyService> notifyService = new Lazy<INotifyService>(
            () => ApplicationContext.Current.Container.Resolve<INotifyService>());

        /// <inheritdoc />
        public override Task OnConnectedAsync()
        {
            var contextId = this.Context.ConnectionId;
            var identity = this.Context.User.Identity;
            if (identity.IsAuthenticated)
            {
                this.notifyService.Value.Connect(contextId, identity);
                return Task.CompletedTask;
            }

            return base.OnConnectedAsync();
        }

        /// <inheritdoc />
        public override Task OnDisconnectedAsync(Exception exception)
        {
            this.notifyService.Value.Disconnect(this.Context.ConnectionId);

            return base.OnDisconnectedAsync(exception);
        }

        public Task<IDataResult> OnButtonClick(long messageId, ButtonType button)
        {
            var contextId = this.Context.ConnectionId;
            return Task.Run(() => this.notifyService.Value.ButtonClick(contextId, messageId, button));
        }
    }
}