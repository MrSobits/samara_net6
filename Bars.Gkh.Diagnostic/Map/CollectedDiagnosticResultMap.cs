namespace Bars.Gkh.Diagnostic.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using Bars.Gkh.Diagnostic.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    public class CollectedDiagnosticResultMap : BaseEntityMap<CollectedDiagnosticResult>
    {
        public CollectedDiagnosticResultMap()
            : base("Результат диагностики системы", "GKH_COLLECTED_DIAGNOSTIC_RESULT")
        {
        }

        protected override void Map()
        {
            Property(x => x.State, "Результат проведения диагностики системы").Column("STATE");
        }
    }
    public class CollectedDiagnosticResultNHibernateMapping : ClassMapping<CollectedDiagnosticResult>
    {
        public CollectedDiagnosticResultNHibernateMapping()
        {
            Bag(
                x => x.DiagnosticResults,
                m =>
                {
                    m.Access(Accessor.NoSetter);
                    m.Key(k => k.Column("COLLECTED_DIAGNOSTIC_ID"));
                    m.Lazy(CollectionLazy.Lazy);
                    m.Fetch(CollectionFetchMode.Select);
                },
                action => action.OneToMany());
        }
    }
}
