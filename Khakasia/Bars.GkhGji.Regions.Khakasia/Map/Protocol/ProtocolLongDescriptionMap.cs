namespace Bars.GkhGji.Regions.Khakasia.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Khakasia.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Khakasia.Entities.ProtocolLongDescription"</summary>
    public class ProtocolLongDescriptionMap : BaseEntityMap<ProtocolLongDescription>
    {
        
        public ProtocolLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Khakasia.Entities.ProtocolLongDescription", "GJI_PROTOCOL_LONGDESC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Protocol, "Protocol").Column("PROTOCOL_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
        }
    }

    public class ProtocolLongDescriptionNHibernateMapping : ClassMapping<ProtocolLongDescription>
    {
        public ProtocolLongDescriptionNHibernateMapping()
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
