namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Логируемое поле сущности
    /// </summary>
    public class EntityHistoryTranslatedPropertiesMap : PersistentObjectMap<EntityHistoryTranslatedProperties>
    {
        /// <inheritdoc />
        public EntityHistoryTranslatedPropertiesMap()
            : base("Переведенные названия логируемых полей сущности", "GKH_TRANSLATE_ENTITY_HISTORY_FIELD")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.EntityType, "Тип сущности").Column("ENTITY_TYPE");
            this.Property(x => x.EnglishName, "Английское название").Column("ENGLISH_NAME");
            this.Property(x => x.RussianName, "Русское название").Column("RUSSIAN_NAME");
        }
    }
}