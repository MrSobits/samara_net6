/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Zabaykalye.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Zabaykalye.Entities;
/// 
///     using NHibernate.Type;
/// 
///     public class ProtocolLongDescriptionMap : BaseEntityMap<ProtocolLongDescription>
///     {
///         public ProtocolLongDescriptionMap()
///             : base("GJI_PROTOCOL_LONGDESC")
///         {
///             References(x => x.Protocol, "PROTOCOL_ID", ReferenceMapConfig.NotNull);
///             Property(
///                 x => x.Description,
///                 mapper =>
///                 {
///                     mapper.Column("DESCRIPTION");
///                     mapper.Type<BinaryBlobType>();
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Zabaykalye.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Zabaykalye.Entities.ProtocolLongDescription"</summary>
    public class ProtocolLongDescriptionMap : BaseEntityMap<ProtocolLongDescription>
    {
        
        public ProtocolLongDescriptionMap() : 
                base("Bars.GkhGji.Regions.Zabaykalye.Entities.ProtocolLongDescription", "GJI_PROTOCOL_LONGDESC")
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
