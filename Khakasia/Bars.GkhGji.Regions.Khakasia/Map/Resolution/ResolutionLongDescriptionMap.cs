namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.ResolutionLongDescription"</summary>
    public class ResolutionLongDescriptionMap : BaseEntityMap<ResolutionLongDescription>
    {
        
        public ResolutionLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.ResolutionLongDescription", "GJI_RESOLUTION_LONGDESC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Resolution, "Resolution").Column("RESOLUTION_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }

    public class ResolutionLongDescriptionNHibernateMapping : ClassMapping<ResolutionLongDescription>
    {
        public ResolutionLongDescriptionNHibernateMapping()
        {
            Property(
                x => x.Description,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
