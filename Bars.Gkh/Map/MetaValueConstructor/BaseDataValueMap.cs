namespace Bars.Gkh.Map.MetaValueConstructor
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг <see cref="BaseDataValue"/>
    /// </summary>
    public class BaseDataValueMap : BaseEntityMap<BaseDataValue>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public BaseDataValueMap()
            : base("Bars.Gkh.MetaValueConstructor.DomainModel.BaseDataValue", "GKH_CONSTRUCTOR_DATA_VALUE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Value, "Значение").Column("VALUE");
            this.Reference(x => x.MetaInfo, "Описатель").Column("META_INFO_ID").NotNull().Fetch();
            this.Reference(x => x.Parent, "Родительский атрибут").Column("PARENT_ID");
        }
    }

    /// <summary>
    /// Маппинг сериализации значения атрибута
    /// </summary>
    public class BaseDataValueNHibernateMapping : ClassMapping<BaseDataValue>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public BaseDataValueNHibernateMapping()
        {
            this.Property(x => x.Value, m => m.Type<ImprovedBinaryJsonType<object>>());
        }
    }
}