/// <mapping-converter-backup>
/// namespace Bars.Gkh.Regions.Tomsk.Map
/// {
///     using Bars.Gkh.Regions.Tomsk.Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Данная сущность расширяет базовую сущность дополнительными полями 
///     /// </summary>
///     public class TomskOperatorMap: SubclassMap<TomskOperator>
///     {
///         public TomskOperatorMap()
///         {
///             Table("GKH_TOMSK_OPERATOR");
///             KeyColumn("ID");
/// 
///             Map(x => x.ShowUnassigned, "SHOW_UNASSIGNED");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Regions.Tomsk.Entities.TomskOperator"</summary>
    public class TomskOperatorMap : JoinedSubClassMap<TomskOperator>
    {
        
        public TomskOperatorMap() : 
                base("Bars.Gkh.Regions.Tomsk.Entities.TomskOperator", "GKH_TOMSK_OPERATOR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ShowUnassigned, "ShowUnassigned").Column("SHOW_UNASSIGNED");
        }
    }
}
