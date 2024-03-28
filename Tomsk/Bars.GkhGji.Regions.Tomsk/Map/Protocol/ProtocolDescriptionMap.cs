/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class ProtocolDescriptionMap : BaseEntityMap<ProtocolDescription>
///     {
///         public ProtocolDescriptionMap()
///             : base("GJI_TOMSK_PROTOCOL_DESCR")
///         {
///             References(x => x.Protocol, "PROTOCOL_ID", ReferenceMapConfig.NotNull);
///             Property(x => x.Description,
///                 mapper =>
///                 {
///                     mapper.Column("DESCRIPTION");
///                     mapper.Type<BinaryBlobType>();
///                 });
///             Property(x => x.DescriptionSet,
///                 mapper =>
///                 {
///                     mapper.Column("DESCRIPTION_SET");
///                     mapper.Type<BinaryBlobType>();
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.ProtocolDescription"</summary>
    public class ProtocolDescriptionMap : BaseEntityMap<ProtocolDescription>
    {
        
        public ProtocolDescriptionMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.ProtocolDescription", "GJI_TOMSK_PROTOCOL_DESCR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Protocol, "Protocol").Column("PROTOCOL_ID").NotNull();
            Property(x => x.Description, "Description").Column("DESCRIPTION");
            Property(x => x.DescriptionSet, "DescriptionSet").Column("DESCRIPTION_SET");
        }
    }

    public class ProtocolDescriptionNHibernateMapping : ClassMapping<ProtocolDescription>
    {
        public ProtocolDescriptionNHibernateMapping()
        {
            Property(
                x => x.Description,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
            Property(
                x => x.DescriptionSet,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
