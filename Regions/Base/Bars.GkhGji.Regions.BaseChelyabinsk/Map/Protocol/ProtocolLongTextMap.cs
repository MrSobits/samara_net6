/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     using Bars.GkhGji.Regions.Chelyabinsk.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class ProtocolLongTextMap : BaseEntityMap<ProtocolLongText>
///     {
///         public ProtocolLongTextMap()
///             : base("GJI_PROTOCOL_LTEXT")
///         {
///             this.References(x => x.Protocol, "PROTOCOL_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.Description,
///                 mapper =>
///                 {
///                     mapper.Column("DESCRIPTION");
///                     mapper.Type<BinaryBlobType>();
///                 });
///             this.Property(
///                 x => x.Witnesses,
///                 mapper =>
///                 {
///                     mapper.Column("WITNESSES");
///                     mapper.Type<BinaryBlobType>();
///                 });
///             this.Property(
///                 x => x.Victims,
///                 mapper =>
///                 {
///                     mapper.Column("VICTIMS");
///                     mapper.Type<BinaryBlobType>();
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ProtocolLongText"</summary>
    public class ProtocolLongTextMap : BaseEntityMap<ProtocolLongText>
    {
        
        public ProtocolLongTextMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ProtocolLongText", "GJI_PROTOCOL_LTEXT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Protocol, "Protocol").Column("PROTOCOL_ID").NotNull();
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
            this.Property(x => x.Witnesses, "Witnesses").Column("WITNESSES");
            this.Property(x => x.Victims, "Victims").Column("VICTIMS");
        }
    }

    public class ProtocolLongTextNHibernateMapping : ClassMapping<ProtocolLongText>
    {
        public ProtocolLongTextNHibernateMapping()
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
