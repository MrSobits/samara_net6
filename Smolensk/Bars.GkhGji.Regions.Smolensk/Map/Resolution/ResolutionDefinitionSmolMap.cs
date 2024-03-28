/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Smolensk.Map.Resolution
/// {
///     using Bars.GkhGji.Regions.Smolensk.Entities.Resolution;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class ProtocolDefinitionSmolMap : SubclassMap<ResolutionDefinitionSmol>
///     {
///         public ProtocolDefinitionSmolMap()
///         {
///             Table("GJI_RESOLUTION_DEF_SMOL");
///             KeyColumn("ID");
///             Map(x => x.DefinitionResult, "DEF_RESULT").Length(2000);
///             Map(x => x.DescriptionSet, "DESCRIPTION_SET").Length(2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Smolensk.Map.Resolution
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Smolensk.Entities.Resolution;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Smolensk.Entities.Resolution.ResolutionDefinitionSmol"</summary>
    public class ResolutionDefinitionSmolMap : JoinedSubClassMap<ResolutionDefinitionSmol>
    {
        
        public ResolutionDefinitionSmolMap() : 
                base("Bars.GkhGji.Regions.Smolensk.Entities.Resolution.ResolutionDefinitionSmol", "GJI_RESOLUTION_DEF_SMOL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DefinitionResult, "DefinitionResult").Column("DEF_RESULT").Length(2000);
            Property(x => x.DescriptionSet, "DescriptionSet").Column("DESCRIPTION_SET").Length(2000);
        }
    }
}
