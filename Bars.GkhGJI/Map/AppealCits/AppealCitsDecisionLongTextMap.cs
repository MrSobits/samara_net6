namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Определение постановления ГЖИ"</summary>
    public class AppealCitsDecisionLongTextMap : BaseEntityMap<AppealCitsDecisionLongText>
    {
        
        public AppealCitsDecisionLongTextMap() : 
                base("Определение постановления ГЖИ", "GJI_APPCIT_DECISION_LT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Decided, "Дата документа").Column("DECIDED");
            Property(x => x.Established, "Дата документа").Column("ESTABLISHED");
            Reference(x => x.AppealCitsDecision, "ДЛ, вынесшее определение").Column("DECISION_ID").NotNull();
        }
    }
    public class AppealCitsDecisionLongTextNHibernateMapping : ClassMapping<AppealCitsDecisionLongText>
    {
        public AppealCitsDecisionLongTextNHibernateMapping()
        {
            this.Property(
                x => x.Decided,
                mapper =>
                {
                    mapper.Type<BinaryBlobType>();
                });
            this.Property(
                x => x.Established,
                mapper =>
                {
                    mapper.Type<BinaryBlobType>();
                });            
        }
    }
}
