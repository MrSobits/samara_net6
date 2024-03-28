/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smol.Map
/// {
///     using Bars.GkhGji.Regions.Smolensk.Entities.Protocol;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "протокол смоленска"
///     /// </summary>
///     public class ProtocolSmolMap : SubclassMap<ProtocolSmol>
///     {
///         public ProtocolSmolMap()
///         {
///             this.Table("GJI_PROTOCOL_SMOL");
///             this.KeyColumn("ID");
///             this.Map(x => x.NoticeDocNumber, "NOTICE_NUMBER").Length(100);
///             this.Map(x => x.NoticeDocDate, "NOTICE_DATE");
///             this.Map(x => x.ViolationDescription, "VIOLATION_DESCIPTION").Length(20000);
///             this.Map(x => x.ExplanationsComments, "EXPLANATIONS_COMMENTS").Length(20000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Smolensk.Map.Protocol
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Smolensk.Entities.Protocol;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.Protocol.ProtocolSmol"</summary>
    public class ProtocolSmolMap : JoinedSubClassMap<ProtocolSmol>
    {
        
        public ProtocolSmolMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.Protocol.ProtocolSmol", "GJI_PROTOCOL_SMOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.NoticeDocNumber, "NoticeDocNumber").Column("NOTICE_NUMBER").Length(100);
            Property(x => x.NoticeDocDate, "NoticeDocDate").Column("NOTICE_DATE");
            Property(x => x.ViolationDescription, "ViolationDescription").Column("VIOLATION_DESCIPTION").Length(20000);
            Property(x => x.ExplanationsComments, "ExplanationsComments").Column("EXPLANATIONS_COMMENTS").Length(20000);
        }
    }
}
