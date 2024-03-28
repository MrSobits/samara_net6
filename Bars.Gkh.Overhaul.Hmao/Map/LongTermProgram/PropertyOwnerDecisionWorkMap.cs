/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class PropertyOwnerDecisionWorkMap : BaseImportableEntityMap<PropertyOwnerDecisionWork>
///     {
///         public PropertyOwnerDecisionWorkMap()
///             : base("OVRHL_PROP_OWN_DECISION_WORK")
///         {
///             References(x => x.Decision, "PROP_OWN_DECISION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Work, "WORK_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Работа по КР решения собственников помещений МКД"</summary>
    public class PropertyOwnerDecisionWorkMap : BaseImportableEntityMap<PropertyOwnerDecisionWork>
    {
        
        public PropertyOwnerDecisionWorkMap() : 
                base("Работа по КР решения собственников помещений МКД", "OVRHL_PROP_OWN_DECISION_WORK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Decision, "Решение собственников помещений МКД").Column("PROP_OWN_DECISION_ID").NotNull().Fetch();
            Reference(x => x.Work, "Работа").Column("WORK_ID").NotNull().Fetch();
        }
    }
}
