/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class ListServicesDecisionMap : BaseJoinedSubclassMap<ListServicesDecision>
///     {
///         public ListServicesDecisionMap()
///             : base("OVRHL_PR_DEC_LIST_SERVICES", "ID")
///         {
///             Map(x => x.DateStart, "DATE_START", false);
///             Map(x => x.DateEnd, "DATE_END", false);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.ListServicesDecision"</summary>
    public class ListServicesDecisionMap : JoinedSubClassMap<ListServicesDecision>
    {
        
        public ListServicesDecisionMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.ListServicesDecision", "OVRHL_PR_DEC_LIST_SERVICES")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
        }
    }
}
