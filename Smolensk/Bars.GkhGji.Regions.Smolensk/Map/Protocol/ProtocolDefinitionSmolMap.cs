/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smolensk.Map.Protocol
/// {
///     using Entities.Protocol;
///     using FluentNHibernate.Mapping;
/// 
///     public class ProtocolDefinitionSmolMap : SubclassMap<ProtocolDefinitionSmol>
///     {
///         public ProtocolDefinitionSmolMap()
///         {
///             Table("GJI_PROTOCOL_DEF_SMOL");
///             KeyColumn("ID");
///             Map(x => x.DefinitionResult, "DEF_RESULT").Length(2000);
///             Map(x => x.DescriptionSet, "DESCRIPTION_SET").Length(2000);
///             Map(x => x.ExtendUntil, "EXTEND_UNTIL");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Smolensk.Map.Protocol
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Smolensk.Entities.Protocol;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.Protocol.ProtocolDefinitionSmol"</summary>
    public class ProtocolDefinitionSmolMap : JoinedSubClassMap<ProtocolDefinitionSmol>
    {
        
        public ProtocolDefinitionSmolMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.Protocol.ProtocolDefinitionSmol", "GJI_PROTOCOL_DEF_SMOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DefinitionResult, "DefinitionResult").Column("DEF_RESULT").Length(2000);
            Property(x => x.DescriptionSet, "DescriptionSet").Column("DESCRIPTION_SET").Length(2000);
            Property(x => x.ExtendUntil, "ExtendUntil").Column("EXTEND_UNTIL");
        }
    }
}
