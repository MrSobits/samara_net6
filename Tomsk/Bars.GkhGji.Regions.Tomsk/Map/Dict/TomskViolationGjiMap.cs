/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map.Dict
/// {
///     using Bars.GkhGji.Regions.Tomsk.Entities.Dict;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class TomskViolationGjiMap : SubclassMap<TomskViolationGji>
///     {
///         public TomskViolationGjiMap()
///         {
///             this.Table("GJI_TOMSK_DICT_VIOLATION");
///             this.KeyColumn("ID");
///             this.Map(x => x.RuleOfLaw, "RULE_OF_LAW").Length(2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities.Dict;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.Dict.TomskViolationGji"</summary>
    public class TomskViolationGjiMap : JoinedSubClassMap<TomskViolationGji>
    {
        
        public TomskViolationGjiMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.Dict.TomskViolationGji", "GJI_TOMSK_DICT_VIOLATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.RuleOfLaw, "RuleOfLaw").Column("RULE_OF_LAW").Length(2000);
        }
    }
}
