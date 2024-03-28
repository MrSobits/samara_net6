namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Маппинг сущности наследованной от <see cref="BaseImportableEntity"/>
    /// </summary>
    public abstract class BaseImportableEntityMap<TBaseEntity> : BaseEntityMap<TBaseEntity>
        where TBaseEntity : BaseImportableEntity
    {
        /// <inheritdoc />
        protected BaseImportableEntityMap(string tableName)
            : base(typeof(TBaseEntity).FullName, tableName)
        {
        }

        /// <inheritdoc />
        protected BaseImportableEntityMap(string entityName, string tableName)
            : base(entityName, tableName)
        {
        }

        public override void InitMap()
        {
            base.InitMap();
            this.Property(x => x.ImportEntityId, "Идентификатор сущности внешней системы").Column("IMPORT_ENTITY_ID");
        }
    }
}