namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.Protocol197LongText"</summary>
    public class Protocol197LongTextMap : BaseEntityMap<Protocol197LongText>
    {
        public Protocol197LongTextMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.Protocol197LongText", "GJI_PROTOCOL197_LTEXT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Protocol197, "Protocol197").Column("PROTOCOL_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.Witnesses, "Witnesses").Column("WITNESSES");
            Property(x => x.Victims, "Victims").Column("VICTIMS");
        }
    }

    public class Protocol197LongTextNHibernateMapping : ClassMapping<Protocol197LongText>
    {
        public Protocol197LongTextNHibernateMapping()
        {
            Property(
                x => x.Description,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
            Property(
                x => x.Witnesses,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
            Property(
                x => x.Victims,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
