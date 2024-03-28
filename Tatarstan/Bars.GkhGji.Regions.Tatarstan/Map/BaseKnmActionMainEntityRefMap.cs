namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;

    /// <summary>
    /// Базовый класс маппинга сущностей
    /// наследованных от <see cref="BaseKnmActionMainEntityRef"/>
    /// </summary>
    /// <typeparam name="TEntity">Тип сущности-ссылки</typeparam>
    /// <typeparam name="TMainEntity">Тип основной сущности</typeparam>
    public abstract class BaseKnmActionMainEntityRefMap<TEntity, TMainEntity> : BaseEntityMap<TEntity>
        where TEntity : BaseKnmActionMainEntityRef<TMainEntity>
        where TMainEntity : BaseEntity
    {
        /// <inheritdoc />
        public BaseKnmActionMainEntityRefMap(string entityName, string tableName)
            : base(entityName, tableName)
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.KnmAction, "Действие").Column("KNM_ACTION_ID").NotNull();
            this.Reference(x => x.MainEntity, this.MainEntityName()).Column(this.MainEntityColumn()).NotNull();
        }

        /// <summary>
        /// Наименование основной сущности
        /// </summary>
        protected abstract string MainEntityName();

        /// <summary>
        /// Наименование колонки основной сущности
        /// </summary>
        protected abstract string MainEntityColumn();
    }
}