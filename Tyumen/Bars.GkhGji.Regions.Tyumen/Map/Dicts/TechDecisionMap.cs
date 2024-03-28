/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tyumen.Map.Dicts
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Tyumen.Entities.Dicts;
/// 
///     public class TechDecisionMap : BaseEntityMap<TechDecision>
///     {
///         public TechDecisionMap() : base("GKH_DICT_TECH_DECISION")
///         {
///             Map(x => x.Name, "NAME", true, 200);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tyumen.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tyumen.Entities.Dicts;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tyumen.Entities.Dicts.TechDecision"</summary>
    public class TechDecisionMap : BaseEntityMap<TechDecision>
    {
        
        public TechDecisionMap() : 
                base("Bars.GkhGji.Regions.Tyumen.Entities.Dicts.TechDecision", "GKH_DICT_TECH_DECISION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME").Length(200).NotNull();
        }
    }
}
