namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг сущности наследованной от <see cref="BaseGkhDict"/>
    /// </summary>
    public abstract class BaseGkhDictMap<T> : BaseEntityMap<T>
        where T : BaseGkhDict
    {
        /// <inheritdoc />
        protected BaseGkhDictMap(string entityName, string tableName)
            : base(entityName, tableName)
        {
        }

        public override void InitMap()
        {
            base.InitMap();
            this.Property(x => x.Code, "Код").Column("CODE").NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull();
        }

        protected override void Map()
        {
        }
    }
}