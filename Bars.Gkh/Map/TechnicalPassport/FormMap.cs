namespace Bars.Gkh.Map.TechnicalPassport
{
    using Bars.Gkh.Entities.TechnicalPassport;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    public class FormMap : GkhBaseEntityMap<Form>
    {
        public FormMap()
            : base("TP_FORM")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Название").Column("NAME");
            this.Property(x => x.Title, "Заголовок").Column("TITLE");
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Order, "Порядковый номер").Column("ORDER");
            this.Property(x => x.TableName, "Наименование таблицы").Column("TABLE_NAME");
            this.Reference(x => x.Section, "Секция").Column("SECTION_ID");
            this.Property(x => x.Type, "Тип").Column("TYPE");
        }
    }

    public class FormNHibernateMapping : ClassMapping<Form>
    {
        public FormNHibernateMapping()
        {
            this.Bag(
                x => x.Attributes,
                mapper =>
                {
                    mapper.Access(Accessor.NoSetter);
                    mapper.Fetch(CollectionFetchMode.Select);
                    mapper.Lazy(CollectionLazy.Lazy);
                    mapper.Key(x => x.Column("FORM_ID"));
                    mapper.Cascade(Cascade.None);
                },
                action => action.OneToMany());
        }
    }
}