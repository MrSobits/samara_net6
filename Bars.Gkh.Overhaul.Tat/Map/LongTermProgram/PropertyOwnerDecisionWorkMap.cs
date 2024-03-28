/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class PropertyOwnerDecisionWorkMap : BaseEntityMap<PropertyOwnerDecisionWork>
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

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.PropertyOwnerDecisionWork"</summary>
    public class PropertyOwnerDecisionWorkMap : BaseEntityMap<PropertyOwnerDecisionWork>
    {
        
        public PropertyOwnerDecisionWorkMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.PropertyOwnerDecisionWork", "OVRHL_PROP_OWN_DECISION_WORK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Decision, "Decision").Column("PROP_OWN_DECISION_ID").NotNull().Fetch();
            Reference(x => x.Work, "Work").Column("WORK_ID").NotNull().Fetch();
        }
    }
}
