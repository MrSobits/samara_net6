/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     using Bars.GkhGji.Regions.Nso.Entities;
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

namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.ProtocolLongText"</summary>
    public class ProtocolLongTextMap : BaseEntityMap<ProtocolLongText>
    {
        
        public ProtocolLongTextMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.ProtocolLongText", "GJI_PROTOCOL_LTEXT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Protocol, "Protocol").Column("PROTOCOL_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.Witnesses, "Witnesses").Column("WITNESSES");
            Property(x => x.Victims, "Victims").Column("VICTIMS");
        }
    }

    public class ProtocolLongTextNHibernateMapping : ClassMapping<ProtocolLongText>
    {
        public ProtocolLongTextNHibernateMapping()
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
