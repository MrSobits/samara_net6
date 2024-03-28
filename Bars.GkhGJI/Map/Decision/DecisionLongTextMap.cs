
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ActCheckViolationLongText"</summary>
    public class DecisionLongTextMap : BaseEntityMap<DecisionLongText>
    {
        
        public DecisionLongTextMap() : 
                base("Bars.GkhGji.Entities.DecisionLongText", "GJI_DECISION_LTEXT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Decision, "Decision").Column("DEC_ID").NotNull();
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }

    public class DecisionLongTextNHibernateMapping : ClassMapping<DecisionLongText>
    {
        public DecisionLongTextNHibernateMapping()
        {
            this.Property(
                x => x.Description,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
