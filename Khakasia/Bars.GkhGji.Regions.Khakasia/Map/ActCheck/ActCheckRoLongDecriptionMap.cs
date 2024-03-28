namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.ActCheckRoLongDescription"</summary>
    public class ActCheckRoLongDescriptionMap : BaseEntityMap<ActCheckRoLongDescription>
    {
        
        public ActCheckRoLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.ActCheckRoLongDescription", "GJI_ACTCHECKRO_LTEXT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ActCheckRo, "ActCheckRo").Column("ACTCHECK_RO_ID").NotNull();
            Property(x => x.NotRevealedViolations, "NotRevealedViolations").Column("NOT_REV_VIOL");
        }
    }

    public class ActCheckRoLongDescriptionNHibernateMapping : ClassMapping<ActCheckRoLongDescription>
    {
        public ActCheckRoLongDescriptionNHibernateMapping()
        {
            Property(
                x => x.NotRevealedViolations,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
