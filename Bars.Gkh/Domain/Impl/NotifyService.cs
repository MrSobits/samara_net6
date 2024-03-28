namespace Bars.Gkh.Domain.Impl
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Security.Principal;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Administration.Notification;
    using Bars.Gkh.Enums.Notification;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.SignalR;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;

    using Microsoft.AspNetCore.SignalR;
    using Microsoft.Extensions.Logging;

    using Newtonsoft.Json;

    using Npgsql;

    /// <inheritdoc />
    public class NotifyService : INotifyService
    {
        private readonly ConcurrentDictionary<string, SignalRUserIdentity> users 
            = new ConcurrentDictionary<string, SignalRUserIdentity>();

        private readonly IHubContext<NotifyHub, INotifyHubClient> hub;

        private readonly IWindsorContainer Container;
        public IRepository<NotifyMessage> NotifyMessageRepository { get; set; }
        public IRepository<NotifyStats> NotifyStatsRepository { get; set; }
        public IRepository<NotifyPermission> NotifyPermissionRepository { get; set; }
        public IRepository<UserRole> UserRoleRepository { get; set; }
        public ILogger LogManager { get; set; }

        public NotifyService(IWindsorContainer container)
        {
            this.Container = container;
            this.hub = container.Resolve<IHubContext<NotifyHub, INotifyHubClient>>();
        }

        /// <inheritdoc />
        public void Connect(string connectionId, IIdentity identity)
        {
            try
            {
                using (this.Container.BeginScope())
                {
                    var userInfo = this.UserRoleRepository.GetAll()
                        .Where(x => x.User.Login == identity.Name)
                        .Select(x => new
                        {
                            UserId = x.User.Id,
                            RoleName = x.Role.Name
                        })
                        .Single();

                    var userIdentity = new SignalRUserIdentity(userInfo.UserId, identity.Name, userInfo.RoleName);
                    if (this.users.TryAdd(connectionId, userIdentity))
                    {
                        this.hub.Groups.AddToGroupAsync(connectionId, userInfo.RoleName).GetResultWithoutContext();
                    }

                    this.SendAllFor(connectionId);
                }
            }
            catch (Exception e)
            {
                this.LogManager.LogError(e, "Не удалось установить подключение пользователя");
            }
        }

        /// <inheritdoc />
        public void Disconnect(string connectionId)
        {
            SignalRUserIdentity removedIdentity;
            if (this.users.TryRemove(connectionId, out removedIdentity))
            {
                this.hub.Groups.RemoveFromGroupAsync(connectionId, removedIdentity.RoleName).GetResultWithoutContext();
            }
        }

        /// <inheritdoc />
        public IDataResult SendForAll(NotifyMessage message)
        {
            this.hub.Clients.All.SendMessage(JsonConvert.SerializeObject(message)).GetResultWithoutContext();

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult Send(NotifyMessage message, User user)
        {
            this.hub.Clients.User(user.Login).SendMessage(JsonConvert.SerializeObject(message)).GetResultWithoutContext();

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult Send(NotifyMessage message, Role role)
        {
            this.UserRoleRepository.GetAll()
                .Where(x => x.Role.Id == role.Id)
                .Select(x => x.User.Login)
                .ToList()
                .ForEach(login => this.hub.Clients.User(login).SendMessage(JsonConvert.SerializeObject(message)).GetResultWithoutContext());

            return new BaseDataResult();
        }

        private void SendAllFor(string connectionId)
        {
            SignalRUserIdentity identity;
            var client = this.hub.Clients.Client(connectionId);
            if (!this.users.TryGetValue(connectionId, out identity) || client == null)
            {
                return;
            }

            var clickExistsQuery = this.NotifyStatsRepository.GetAll()
                .Where(x => x.User.Id == identity.UserId)
                .Select(x => x.Message.Id);

            var messages = this.NotifyMessageRepository.GetAll()
                .Where(x => !x.IsDelete)
                .Where(x => x.StartDate <= DateTime.Today && DateTime.Today <= x.EndDate)
                .Where(x => !clickExistsQuery.Any(id => id == x.Id))
                .ToList();

            var roleQuery = this.UserRoleRepository.GetAll()
                .Where(x => x.User.Id == identity.UserId)
                .Select(x => x.Role.Id);

            var permissionMap = this.NotifyPermissionRepository.GetAll()
                .WhereContainsBulked(x => x.Message.Id, messages.Select(x => x.Id))
                .Select(x => new
                {
                    x.Message.Id,
                    IsAllow = roleQuery.Any(id => id == x.Role.Id)
                })
                .AsEnumerable()
                .GroupBy(x => x.Id, x => x.IsAllow)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault(y => y));

            foreach (var message in messages.Where(x => permissionMap.Get<long, bool>(x.Id, true)))
            {
                client.SendMessage(JsonConvert.SerializeObject(message)).GetResultWithoutContext();
            }
        }

        /// <inheritdoc />
        public IDataResult ButtonClick(string connectionId, long messageId, ButtonType button)
        {
            SignalRUserIdentity identity;
            if (!this.users.TryGetValue(connectionId, out identity))
            {
                return BaseDataResult.Error("Не найден пользователь");
            }

            var stats = new NotifyStats
            {
                ClickButton = button,
                Message = new NotifyMessage { Id = messageId },
                User = new User { Id = identity.UserId }
            };

            try
            {
                using (this.Container.BeginScope(identity))
                {
                    this.NotifyStatsRepository.Save(stats);
                }
            }
            catch (PostgresException e) when (e.ConstraintName == "gkh_notify_stats_unique")
            {
                this.LogManager.LogDebug($"Дублированное нажатие пользователем '{identity.Name}' на уведомление({messageId})");
            }
            catch (Exception e)
            {
                var msg = "Не удалось сохранить ответ пользователя";
                this.LogManager.LogError(e, msg);
                return BaseDataResult.Error(msg);
            }

            this.hub.Clients.User(identity.Name).CloseWindow(JsonConvert.SerializeObject(new { messageId })).GetResultWithoutContext();

            return new BaseDataResult();
        }

        private class SignalRUserIdentity : IUserIdentity
        {
            public SignalRUserIdentity(long userId, string name, string roleName = null)
            {
                this.UserId = userId;
                this.Name = name;
                this.RoleName = roleName ?? string.Empty;
                this.TrackId = Guid.NewGuid().ToString("N");
            }

            /// <inheritdoc />
            public string AuthenticationType => "SignalR.NotifyService";

            /// <inheritdoc />
            public bool IsAuthenticated => true;

            /// <inheritdoc />
            public string Name { get; }

            /// <inheritdoc />
            public long UserId { get; }

            /// <summary>
            /// Наименование роли
            /// </summary>
            public string RoleName { get; }

            /// <inheritdoc />
            public string TrackId { get; }
        }
    }
}