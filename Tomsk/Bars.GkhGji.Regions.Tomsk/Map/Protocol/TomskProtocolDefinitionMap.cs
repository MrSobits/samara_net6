/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class TomskProtocolDefinitionMap : SubclassMap<TomskProtocolDefinition>
///     {
///         public TomskProtocolDefinitionMap()
///         {
///             Table("GJI_TOMSK_PROTOCOL_DEF");
///             KeyColumn("ID");
///             Map(x => x.PlaceReview, "PLACE_REVIEW").Length(2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.TomskProtocolDefinition"</summary>
    public class TomskProtocolDefinitionMap : JoinedSubClassMap<TomskProtocolDefinition>
    {
        
        public TomskProtocolDefinitionMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.TomskProtocolDefinition", "GJI_TOMSK_PROTOCOL_DEF")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PlaceReview, "PlaceReview").Column("PLACE_REVIEW").Length(2000);
        }
    }
}
