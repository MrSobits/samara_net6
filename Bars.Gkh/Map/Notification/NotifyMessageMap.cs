namespace Bars.Gkh.Map.Notification
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Administration.Notification;

    public class NotifyMessageMap : BaseEntityMap<NotifyMessage>
    {
        /// <inheritdoc />
        public NotifyMessageMap()
            : base("Сообщение-уведомление", "GKH_NOTIFY_MESSAGE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.IsDelete, "Удалено").Column("IS_DELETE").NotNull();
            this.Property(x => x.StartDate, "Актуально с").Column("START_DATE").NotNull();
            this.Property(x => x.EndDate, "Актуально по").Column("END_DATE").NotNull();
            this.Property(x => x.Title, "Заголовок сообщения").Column("TITLE").NotNull();
            this.Property(x => x.Text, "Текст сообщения").Column("TEXT").NotNull();
            this.Property(x => x.ButtonSet, "Набор кнопок").Column("BUTTON_SET").NotNull();

            this.Reference(x => x.User, "Отправитель").Column("USER_ID").NotNull();
        }
    }
}