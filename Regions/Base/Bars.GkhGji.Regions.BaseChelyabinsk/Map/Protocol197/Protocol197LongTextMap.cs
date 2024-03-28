namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol197
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.Protocol197LongText"</summary>
    public class Protocol197LongTextMap : BaseEntityMap<Protocol197LongText>
    {
        public Protocol197LongTextMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.Protocol197LongText", "GJI_PROTOCOL197_LTEXT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Protocol197, "Protocol197").Column("PROTOCOL_ID").NotNull();
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
            this.Property(x => x.Witnesses, "Witnesses").Column("WITNESSES");
            this.Property(x => x.Victims, "Victims").Column("VICTIMS");
        }
    }

    public class Protocol197LongTextNHibernateMapping : ClassMapping<Protocol197LongText>
    {
        public Protocol197LongTextNHibernateMapping()
        {
            this.Property(
                x => x.Description,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
            this.Property(
                x => x.Witnesses,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
            this.Property(
                x => x.Victims,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
