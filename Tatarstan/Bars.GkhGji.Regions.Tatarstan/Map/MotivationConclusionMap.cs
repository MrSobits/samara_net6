namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>
    /// Маппинг полей сущности <see cref="MotivationConclusion"/>
    /// </summary>
    public class MotivationConclusionMap : JoinedSubClassMap<MotivationConclusion>
    {
        /// <inheritdoc />
        public MotivationConclusionMap()
            : base("Мотивировочное заключение", "GJI_MOTIVATION_CONCLUSION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.BaseDocument, "Базовый документ").Column("BASE_DOC_ID");
            this.Reference(x => x.Autor, "Должностное лицо (ДЛ) вынесшее распоряжение").Column("AUTOR_ID");
            this.Reference(x => x.Executant, "Ответственный за исполнение").Column("EXECUTANT_ID");
        }
    }

    public class MotivationConclusionNhMap : JoinedSubclassMapping<MotivationConclusion>
    {
        /// <inheritdoc />
        public MotivationConclusionNhMap()
        {
            this.Bag(x => x.AnnexList,
                m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.Fetch(CollectionFetchMode.Select);
                    m.Lazy(CollectionLazy.Lazy);
                    m.Key(x => x.Column("DOC_ID"));
                    m.Cascade(Cascade.DeleteOrphans);
                    m.Inverse(true);
                },
                x => x.OneToMany());
        }
    }
}
