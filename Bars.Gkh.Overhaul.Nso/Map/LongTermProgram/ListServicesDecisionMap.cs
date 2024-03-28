/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class ListServicesDecisionMap : BaseJoinedSubclassMap<ListServicesDecision>
///     {
///         public ListServicesDecisionMap()
///             : base("OVRHL_PR_DEC_LIST_SERVICES", "ID")
///         {
///             this.Map(x => x.DateStart, "DATE_START", false);
///             this.Map(x => x.DateEnd, "DATE_END", false);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.ListServicesDecision"</summary>
    public class ListServicesDecisionMap : JoinedSubClassMap<ListServicesDecision>
    {
        
        public ListServicesDecisionMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.ListServicesDecision", "OVRHL_PR_DEC_LIST_SERVICES")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
        }
    }
}
