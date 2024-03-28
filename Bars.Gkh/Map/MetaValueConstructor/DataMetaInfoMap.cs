namespace Bars.Gkh.Map.MetaValueConstructor
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.MetaValueConstructor.DomainModel;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг <see cref="DataMetaInfo"/>
    /// </summary>
    public class DataMetaInfoMap : BaseEntityMap<DataMetaInfo>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public DataMetaInfoMap()
            : base("Bars.Gkh.MetaValueConstructor.DomainModel.DataValue", "GKH_CONSTRUCTOR_DATA_META")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").NotNull().Length(255);
            this.Property(x => x.Code, "Код").Column("CODE").NotNull().Length(60);
            this.Property(x => x.Weight, "Вес").Column("WEIGHT");
            this.Property(x => x.Formula, "Формула").Column("FORMULA").Length(255);
            this.Property(x => x.Level, "Уровень").Column("LEVEL").NotNull();
            this.Property(x => x.DataValueType, "Тип значения").Column("TYPE").NotNull();
            this.Property(x => x.MinLength, "Минимальная длина(для строки)").Column("MIN_LENGTH");
            this.Property(x => x.MaxLength, "Максимальная длина(для строки)").Column("MAX_LENGTH");
            this.Property(x => x.Decimals, "Количество знаков после запятой").Column("DECIMALS");
            this.Property(x => x.Required, "Обязательность").Column("REQUIRED").NotNull();
            this.Property(x => x.DataFillerName, "Источник данных").Column("FILLER_NAME");

            this.Reference(x => x.Parent, "Родительский атрибут").Column("PARENT_ID");
            this.Reference(x => x.Group, "Группа конструктора").Column("GROUP_ID").NotNull().Fetch();
        }
    }

    /// <summary>
    /// Маппинг мета-описания атрибутов
    /// </summary>
    public class DataMetaInfoNHibernateMapping : ClassMapping<DataMetaInfo>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public DataMetaInfoNHibernateMapping()
        {
            this.Bag(
                x => x.Children,
                mapper =>
                {
                    mapper.Access(Accessor.NoSetter);
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Lazy(CollectionLazy.Lazy);
                    mapper.Key(
                        k =>
                        {
                            k.Column("PARENT_ID");
                        });
                    mapper.Cascade(Cascade.Persist);
                    mapper.Inverse(true);
                },
                action => action.OneToMany(x => x.Class(typeof(DataMetaInfo))));
        }
    }
}