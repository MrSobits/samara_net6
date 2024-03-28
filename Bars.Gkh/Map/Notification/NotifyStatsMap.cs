namespace Bars.Gkh.Map.Notification
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Administration.Notification;

    public class NotifyStatsMap : BaseEntityMap<NotifyStats>
    {
        /// <inheritdoc />
        public NotifyStatsMap()
            : base("Статистика уведомлений", "GKH_NOTIFY_STATS")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ClickButton, "Нажатая кнопка").Column("BUTTON").NotNull();
            this.Reference(x => x.Message, "Сообщение").Column("MESSAGE_ID").NotNull();
            this.Reference(x => x.User, "Пользователь").Column("USER_ID").NotNull();
        }
    }
}