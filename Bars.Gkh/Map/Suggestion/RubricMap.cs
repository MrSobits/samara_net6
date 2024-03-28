/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Suggestion
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Suggestion;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class RubricMap : BaseImportableEntityMap<Rubric>
///     {
///         public RubricMap() : base("GKH_SUG_RUBRIC")
///         {
///             Map(x => x.ExternalId, "EXTERNAL_ID");
///             Map(x => x.Code, "CODE");
///             Map(x => x.Name, "NAME");
///             Map(x => x.FirstExecutorType, "EXECUTOR_TYPE");
///             Map(x => x.IsActual, "IS_ACTUAL");
///             Map(x => x.ExpireSuggestionTerm, "EXPIRE_SUGGESTION_TERM");
/// 
///             Bag(x => x.Transitions, mapper =>
///             {
///                 mapper.Access(Accessor.Property);
///                 mapper.Key(keyMapper => keyMapper.Column("RUBRIC_ID"));
///                 mapper.Cascade(Cascade.None);
///                 mapper.Fetch(CollectionFetchMode.Select);
///                 mapper.Lazy(CollectionLazy.Lazy);
///                 mapper.Inverse(true);
///             }, action => action.OneToMany());
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Suggestion;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Рубрика обращения граждан. Служит для группировки обращений и определяет процесс, по правилам которого будут обрабатываться обращения."</summary>
    public class RubricMap : BaseImportableEntityMap<Rubric>
    {

        public RubricMap()
            : base(
                "Рубрика обращения граждан. Служит для группировки обращений и определяет процесс,"
                + " по правилам которого будут обрабатываться обращения.",
                "GKH_SUG_RUBRIC")
        {
        }

        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(250);
            Property(x => x.Code, "Код").Column("CODE");
            Property(x => x.Name, "Наименование").Column("NAME").Length(250);
            Property(x => x.FirstExecutorType, "Тип первого исполнителя").Column("EXECUTOR_TYPE");
            Property(x => x.IsActual, "Активна").Column("IS_ACTUAL");
            Property(x => x.ExpireSuggestionTerm, "Срок(дни) автоматического перевода обращения в статус \"Выполнено\"")
                .Column("EXPIRE_SUGGESTION_TERM");
        }
    }

    public class RubricNHibernateMapping : ClassMapping<Rubric>
    {
        public RubricNHibernateMapping()
        {
            Bag(
                x => x.Transitions,
                mapper =>
                    {
                        mapper.Access(Accessor.Property);
                        mapper.Key(keyMapper => keyMapper.Column("RUBRIC_ID"));
                        mapper.Cascade(Cascade.None);
                        mapper.Fetch(CollectionFetchMode.Select);
                        mapper.Lazy(CollectionLazy.Lazy);
                        mapper.Inverse(true);
                    },
                action => action.OneToMany());
        }
    }
}