namespace Bars.Gkh.Map.Notification
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Administration.Notification;

    public class NotifyPermissionMap : PersistentObjectMap<NotifyPermission>
    {
        /// <inheritdoc />
        public NotifyPermissionMap()
            : base("Права доступа для роли на уведомление", "GKH_NOTIFY_PERMISSION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Message, "Сообщение").Column("MESSAGE_ID").NotNull();
            this.Reference(x => x.Role, "Роль").Column("ROLE_ID");
        }
    }
}