/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Справочник Статья закона"
///     /// </summary>
///     public class TomskArticleLawGjiMap : SubclassMap<TomskArticleLawGji>
///     {
///         public TomskArticleLawGjiMap()
///         {
///             Table("TOMSK_GJI_ARTICLELAW");
///             KeyColumn("ID");
/// 
///             Map(x => x.PhysPersonPenalty, "PHYS_PENALTY");
///             Map(x => x.JurPersonPenalty, "JUR_PENALTY");
///             Map(x => x.OffPersonPenalty, "OFF_PENALTY");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.TomskArticleLawGji"</summary>
    public class TomskArticleLawGjiMap : JoinedSubClassMap<TomskArticleLawGji>
    {
        
        public TomskArticleLawGjiMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.TomskArticleLawGji", "TOMSK_GJI_ARTICLELAW")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PhysPersonPenalty, "PhysPersonPenalty").Column("PHYS_PENALTY");
            Property(x => x.JurPersonPenalty, "JurPersonPenalty").Column("JUR_PENALTY");
            Property(x => x.OffPersonPenalty, "OffPersonPenalty").Column("OFF_PENALTY");
        }
    }
}
