namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Информация об изменения сущности
    /// </summary>
    public class EntityHistoryInfoMap : PersistentObjectMap<EntityHistoryInfo>
    {
        /// <inheritdoc />
        public EntityHistoryInfoMap()
            : base("Информация об изменения сущности", "GKH_ENTITY_HISTORY_INFO")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.GroupType, "Тип группы истории изменений").Column("GROUP_TYPE").NotNull();
            this.Property(x => x.EditDate, "Дата изменения").Column("EDIT_DATE").NotNull();
            this.Property(x => x.ActionKind, "Действие").Column("ACTION_KIND").NotNull();
            this.Property(x => x.Username, "Имя пользователя").Column("USERNAME");
            this.Property(x => x.IpAddress, "IP адрес").Column("IP_ADDRESS");
            this.Property(x => x.EntityId, "Идентификатор сущности").Column("ENTITY_ID").NotNull();
            this.Property(x => x.EntityName, "Имя сущности").Column("ENTITY_NAME").NotNull();
            this.Property(x => x.ParentEntityId, "Идентификатор родительской сущности").Column("PARENT_ENTITY_ID").NotNull();
            this.Property(x => x.ParentEntityName, "Имя родительской сущности").Column("PARENT_ENTITY_NAME");
            this.Reference(x => x.User, "Пользователь").Column("USER_ID");
        }
    }
}