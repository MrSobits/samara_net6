/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smolensk.Map.Protocol
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Smolensk.Entities.Protocol;
/// 
///     using NHibernate.Type;
/// 
///     public class ProtocolViolationDescriptionMap : BaseEntityMap<ProtocolViolationDescription>
///     {
///         public ProtocolViolationDescriptionMap()
///             : base("GJI_PROTOCOL_SMOL_VIOL_DESCR")
///         {
///             this.References(x => x.Protocol, "PROTOCOL_ID", ReferenceMapConfig.NotNull);
///             this.Property(
///                 x => x.ViolationDescription,
///                 mapper =>
///                     {
///                         mapper.Column("VIOLATION_DESCR");
///                         mapper.Type<BinaryBlobType>();
///                     });
///             this.Property(
///                 x => x.ExplanationsComments,
///                 mapper =>
///                     {
///                         mapper.Column("EXPLANATIONS_COMMENTS");
///                         mapper.Type<BinaryBlobType>();
///                     });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Smolensk.Map.Protocol
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Smolensk.Entities.Protocol;

    using NHibernate.Mapping.ByCode.Conformist;
    using NHibernate.Type;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.Protocol.ProtocolViolationDescription"</summary>
    public class ProtocolViolationDescriptionMap : BaseEntityMap<ProtocolViolationDescription>
    {
        
        public ProtocolViolationDescriptionMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.Protocol.ProtocolViolationDescription", "GJI_PROTOCOL_SMOL_VIOL_DESCR")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Protocol, "Protocol").Column("PROTOCOL_ID").NotNull();
            Property(x => x.ViolationDescription, "ViolationDescription").Column("VIOLATION_DESCR");
            Property(x => x.ExplanationsComments, "ExplanationsComments").Column("EXPLANATIONS_COMMENTS");
        }
    }

    public class ProtocolViolationDescriptionNHibernateMapping : ClassMapping<ProtocolViolationDescription>
    {
        public ProtocolViolationDescriptionNHibernateMapping()
        {
            Property(
                x => x.ViolationDescription,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
            Property(
                x => x.ExplanationsComments,
                mapper =>
                    {
                        mapper.Type<BinaryBlobType>();
                    });
        }
    }
}
