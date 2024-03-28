/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
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

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.ListServiceDecisionWorkPlan"</summary>
    public class ListServiceDecisionWorkPlanMap : BaseEntityMap<ListServiceDecisionWorkPlan>
    {
        
        public ListServiceDecisionWorkPlanMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.ListServiceDecisionWorkPlan", "OVRHL_PRDEC_SVC_WORK_FACT")
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
