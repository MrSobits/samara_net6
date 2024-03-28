namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Определение постановления ГЖИ"</summary>
    public class ResolutionDecisionLongTextMap : BaseEntityMap<ResolutionDecisionLongText>
    {
        
        public ResolutionDecisionLongTextMap() : 
                base("Определение постановления ГЖИ", "GJI_RESOLUTION_DECISION_LT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Decided, "Дата документа").Column("DECIDED");
            Property(x => x.Established, "Дата документа").Column("ESTABLISHED");
            Reference(x => x.ResolutionDecision, "ДЛ, вынесшее определение").Column("DECISION_ID").NotNull();
        }
    }
    public class ResolutionDecisionLongTextNHibernateMapping : ClassMapping<ResolutionDecisionLongText>
    {
        public ResolutionDecisionLongTextNHibernateMapping()
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
