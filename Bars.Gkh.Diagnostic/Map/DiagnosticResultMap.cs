namespace Bars.Gkh.Diagnostic.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Diagnostic.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    public class DiagnosticResultMap : BaseEntityMap<DiagnosticResult>
    {
        public DiagnosticResultMap()
            : base("Результат дипагностики", "GKH_DIAGNOSTIC_RESULT")
        {
        }

        protected override void Map()
        {
            Property(x => x.Name, "Наименование диагностики").Column("NAME");
            Property(x => x.State, "Результат проведения диагностики").Column("STATE");
            Property(x => x.Message, "Сообщение описывающие результат проведения диагностики").Column("MESSAGE").Length(3000);
            Reference(x => x.CollectedDiagnostic, "Диагностика системы").Column("COLLECTED_DIAGNOSTIC_ID").NotNull().Fetch();
        }

        public class DocumentViolGroupLongTextNHibernateMapping : ClassMapping<DiagnosticResult>
        {
            public DocumentViolGroupLongTextNHibernateMapping()
            {
                Property(
                    x => x.Message,
                    mapper => mapper.Type<ImprovedBinaryStringType>());
            }
        }
    }
}
