namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Логируемое поле сущности
    /// </summary>
    public class EntityHistoryFieldMap : PersistentObjectMap<EntityHistoryField>
    {
        /// <inheritdoc />
        public EntityHistoryFieldMap()
            : base("Логируемое поле сущности", "GKH_ENTITY_HISTORY_FIELD")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.OldValue, "Старое значение").Column("OLD_VALUE");
            this.Property(x => x.NewValue, "Новое значение").Column("NEW_VALUE");
            this.Property(x => x.FieldName, "Наименование поля").Column("FIELD_NAME");
            this.Reference(x => x.EntityHistoryInfo, "Информация об изменении").Column("ENTITY_HISTORY_INFO_ID");
        }
    }
}