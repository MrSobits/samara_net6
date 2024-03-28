/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.LongTermProgram
/// {
///     using B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class ListServiceDecisionWorkPlanMap : BaseEntityMap<ListServiceDecisionWorkPlan>
///     {
///         public ListServiceDecisionWorkPlanMap()
///             : base("OVRHL_PRDEC_SVC_WORK_FACT")
///         {
///             Map(x => x.FactYear, "FACT_YEAR", false);
/// 
///             References(x => x.ListServicesDecision, "DECISION_ID", ReferenceMapConfig.CascadeDelete);
///             References(x => x.Work, "WORK_ID", ReferenceMapConfig.CascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.ListServiceDecisionWorkPlan"</summary>
    public class ListServiceDecisionWorkPlanMap : BaseEntityMap<ListServiceDecisionWorkPlan>
    {
        
        public ListServiceDecisionWorkPlanMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.ListServiceDecisionWorkPlan", "OVRHL_PRDEC_SVC_WORK_FACT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ListServicesDecision, "ListServicesDecision").Column("DECISION_ID");
            Reference(x => x.Work, "Work").Column("WORK_ID");
            Property(x => x.FactYear, "FactYear").Column("FACT_YEAR");
        }
    }
}
